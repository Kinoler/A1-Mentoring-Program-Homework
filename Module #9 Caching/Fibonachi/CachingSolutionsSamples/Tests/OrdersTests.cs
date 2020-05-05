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
	public class OrdersTests
	{
		[TestMethod]
		public void MemoryCache()
		{
            PrintCached(new MemoryCache<Order>());
        }

        [TestMethod]
		public void RedisCache()
        {
            PrintCached(new RedisCache<Order>("localhost"));
        }

        public void PrintCached(CacheBase<Order> cacheBase)
        {
            var manager = new OrdersManager(cacheBase);

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(manager.GetItems().Count());
                Thread.Sleep(100);
            }
		}
	}
}
