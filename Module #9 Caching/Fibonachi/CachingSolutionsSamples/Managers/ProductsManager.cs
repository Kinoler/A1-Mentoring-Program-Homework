using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CachingSolutionsSamples.Caches;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Managers
{
	public class ProductsManager : ManagerBase<Product>
	{
        public ProductsManager(CacheBase<Product> cache)
            : base(cache)
        {
        }

        protected override DbSet<Product> GetData(Northwind dbContext)
        {
            return dbContext.Products;
        }
    }
}
