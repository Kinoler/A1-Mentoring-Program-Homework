using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples
{
	public class ProductsManager
	{
		private IProductsCache cache;

		public ProductsManager(IProductsCache cache)
		{
			this.cache = cache;
		}

		public IEnumerable<Product> Getproducts()
		{
			Console.WriteLine("Get products");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var products = cache.Get(user);

			if (products == null)
			{
				Console.WriteLine("From DB");

				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
                    products = dbContext.Products.ToList();
					cache.Set(user, products);
				}
			}

			return products;
		}
	}
}
