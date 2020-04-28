﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class OrdersTests
	{
		[TestMethod]
		public void MemoryCache()
		{
			var categoryManager = new OrdersManager(new OrdersMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetOrders().Count());
				Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void RedisCache()
		{
			var categoryManager = new OrdersManager(new OrdersRedisCache("localhost"));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetOrders().Count());
				Thread.Sleep(100);
			}
		}
	}
}
