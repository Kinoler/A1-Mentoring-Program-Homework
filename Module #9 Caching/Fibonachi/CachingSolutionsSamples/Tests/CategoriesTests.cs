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
	public class CategoriesTests
	{
		[TestMethod]
		public void MemoryCache()
		{
            PrintCached(new MemoryCache<Category>());
        }

		[TestMethod]
		public void RedisCache()
		{
            PrintCached(new RedisCache<Category>("localhost"));
        }
        public void PrintCached(CacheBase<Category> cacheBase)
        {
            var manager = new CategoriesManager(cacheBase);

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(manager.GetItems().Count());
                Thread.Sleep(100);
            }
        }
	}
}
