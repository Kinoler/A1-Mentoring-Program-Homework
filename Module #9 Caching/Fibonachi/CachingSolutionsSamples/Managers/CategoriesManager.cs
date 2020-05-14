using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Caches;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Managers
{
	public class CategoriesManager : CollectionManager<Category>
	{
        public CategoriesManager(EntitiesCache<Category> cache)
            : base(cache)
        {
        }

        protected override DbSet<Category> GetData(Northwind dbContext)
        {
            return dbContext.Categories;
        }
    }
}
