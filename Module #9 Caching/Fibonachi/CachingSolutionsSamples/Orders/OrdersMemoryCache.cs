using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using System.Runtime.Caching;

namespace CachingSolutionsSamples
{
	internal class OrdersMemoryCache : IOrdersCache
	{
		ObjectCache cache = MemoryCache.Default;
		string prefix  = "Cache_Orders";

		public IEnumerable<Order> Get(string forUser)
		{
			return (IEnumerable<Order>) cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Order> orders)
		{
			cache.Set(prefix + forUser, orders, DateTime.Now.AddMinutes(10));
		}
	}
}
