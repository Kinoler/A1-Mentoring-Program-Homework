using System;
using System.Collections.Generic;
using System.IO;
using CachingSolutionsSamples.Helpers;
using StackExchange.Redis;

namespace CachingSolutionsSamples.Caches
{
	public class RedisCache<TCacheItem> : CacheBase<TCacheItem>
	{
		private readonly ConnectionMultiplexer _redisConnection;
        private readonly Serializer<IEnumerable<TCacheItem>> _serializer =
            new Serializer<IEnumerable<TCacheItem>>();

		public RedisCache(string hostName)
		{
			_redisConnection = ConnectionMultiplexer.Connect(hostName);
		}

		public override IEnumerable<TCacheItem> Get(string forUser)
		{
			var db = _redisConnection.GetDatabase();
			byte[] s = db.StringGet(Prefix + forUser);
			return s == null ? null : _serializer.Deserialize(new MemoryStream(s));
        }

		public override void Set(string forUser, IEnumerable<TCacheItem> items)
		{
			var db = _redisConnection.GetDatabase();
			var key = Prefix + forUser;
            var expiry = Policy.AbsoluteExpiration - DateTime.Now;

			if (items == null)
			{
				db.StringSet(key, RedisValue.Null, expiry);
			}
			else
			{
                _serializer.Serialize(items, out var stream);
				db.StringSet(key, stream.ToArray(), expiry);
			}
		}
	}
}
