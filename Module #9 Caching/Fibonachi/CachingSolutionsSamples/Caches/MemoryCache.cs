using System.Collections.Generic;
using System.Runtime.Caching;

namespace CachingSolutionsSamples.Caches
{
    public class MemoryCache<TCacheItem> : CacheBase<TCacheItem>
	{
        private readonly ObjectCache _cache = MemoryCache.Default;

        public override IEnumerable<TCacheItem> Get(string forUser)
		{
			return (IEnumerable<TCacheItem>) _cache.Get(Prefix + forUser);
		}

        public override void Set(string forUser, IEnumerable<TCacheItem> items)
        {
            _cache.Set(Prefix + forUser, items, Policy);
        }
	}
}
