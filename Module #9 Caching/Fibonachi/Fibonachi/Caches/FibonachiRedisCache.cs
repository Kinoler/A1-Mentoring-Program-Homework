using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using StackExchange.Redis;

namespace Fibonachi.Caches
{
    public class FibonachiRedisCache : IFibonachiCache
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly DataContractSerializer _serializer = 
            new DataContractSerializer(typeof(Dictionary<int, long>));

        public FibonachiRedisCache(string hostName)
        {
            _redisConnection = ConnectionMultiplexer.Connect(hostName);
        }

        public Dictionary<int, long> Get(string forUser)
        {
            var db = _redisConnection.GetDatabase();
            byte[] s = db.StringGet(CacheConstants.CachePrefix + forUser);
            if (s == null)
                return null;

            return (Dictionary<int, long>)_serializer
                .ReadObject(new MemoryStream(s));
        }

        public void Set(string forUser, Dictionary<int, long> fibonachiCache)
        {
            var db = _redisConnection.GetDatabase();
            var key = CacheConstants.CachePrefix + forUser;

            if (fibonachiCache == null)
            {
                db.StringSet(key, RedisValue.Null, TimeSpan.FromMinutes(CacheConstants.CacheLiveTimeInMinutes));
            }
            else
            {
                var stream = new MemoryStream();
                _serializer.WriteObject(stream, fibonachiCache);
                db.StringSet(key, stream.ToArray(), TimeSpan.FromMinutes(CacheConstants.CacheLiveTimeInMinutes));
            }
        }

        public void Dispose()
        {
            _redisConnection?.Dispose();
        }
    }
}
