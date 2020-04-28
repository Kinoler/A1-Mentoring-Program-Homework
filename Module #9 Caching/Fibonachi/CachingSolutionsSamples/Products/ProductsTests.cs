using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class ProductsTests
	{
		[TestMethod]
		public void MemoryCache()
		{
			var ProductManager = new ProductsManager(new ProductsMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(ProductManager.Getproducts().Count());
				Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void RedisCache()
		{
			var ProductManager = new ProductsManager(new ProductsRedisCache("localhost"));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(ProductManager.Getproducts().Count());
				Thread.Sleep(100);
			}
		}
	}
}
