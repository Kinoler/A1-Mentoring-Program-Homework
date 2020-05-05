using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Caches;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Managers
{
	public abstract class ManagerBase<TItem> where TItem : class
	{
		private readonly CacheBase<TItem> _cache;

        protected ManagerBase(CacheBase<TItem> cache)
		{
			this._cache = cache;
		}

		public IEnumerable<TItem> GetItems()
		{
			Console.WriteLine("Get Items");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var items = _cache.Get(user);

			if (items == null)
			{
				Console.WriteLine("From DB");

				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;

                    var dbSet = GetData(dbContext);

                    _cache.SetInvalidationCache(
                        dbContext.Database.Connection.ConnectionString, 
                        dbSet.Sql);

					items = dbSet.ToList();
					_cache.Set(user, items);
				}
			}

			return items;
		}

        protected abstract DbSet<TItem> GetData(Northwind dbContext);
    }
}
