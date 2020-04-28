using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using System.Runtime.Caching;

namespace CachingSolutionsSamples
{
	internal class ProductsMemoryCache : IProductsCache
	{
		ObjectCache cache = MemoryCache.Default;
		string prefix  = "Cache_products";

		public IEnumerable<Product> Get(string forUser)
		{
			return (IEnumerable<Product>) cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Product> products)
		{
			cache.Set(prefix + forUser, products, DateTime.Now.AddMinutes(10));
		}
	}
}
