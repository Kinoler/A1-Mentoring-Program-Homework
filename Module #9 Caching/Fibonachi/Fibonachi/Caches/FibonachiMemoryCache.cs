using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Fibonachi.Caches
{
    public class FibonachiMemoryCache : IFibonachiCache
    {
        private readonly ObjectCache _cache = MemoryCache.Default;

        public Dictionary<int, long> Get(string forUser)
        {
            return (Dictionary<int, long>)_cache.Get(CacheConstants.CachePrefix + forUser);
        }

        public void Set(string forUser, Dictionary<int, long> fibonachiCache)
        {
            _cache.Set(CacheConstants.CachePrefix + forUser, fibonachiCache, 
                DateTime.Now.AddMinutes(CacheConstants.CacheLiveTimeInMinutes));
        }

        public void Dispose()
        {

        }
    }
}
