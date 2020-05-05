using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Caches;
using CachingSolutionsSamples.Managers;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class ProductsTests
	{
		[TestMethod]
		public void MemoryCache()
		{
            PrintCached(new MemoryCache<Product>());
        }

		[TestMethod]
		public void RedisCache()
		{
            PrintCached(new RedisCache<Product>("localhost"));
        }

        public void PrintCached(CacheBase<Product> cacheBase)
        {
            var manager = new ProductsManager(cacheBase);

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(manager.GetItems().Count());
                Thread.Sleep(100);
            }
        }
	}
}
