using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples
{
	public class OrdersManager
	{
		private IOrdersCache cache;

		public OrdersManager(IOrdersCache cache)
		{
			this.cache = cache;
		}

		public IEnumerable<Order> GetOrders()
		{
			Console.WriteLine("Get Orders");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var orders = cache.Get(user);

			if (orders == null)
			{
				Console.WriteLine("From DB");

				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
					orders = dbContext.Orders.ToList();
					cache.Set(user, orders);
				}
			}

			return orders;
		}
	}
}
