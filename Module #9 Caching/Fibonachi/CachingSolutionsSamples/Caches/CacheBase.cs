using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Caching;
using CachingSolutionsSamples.Helpers;

namespace CachingSolutionsSamples.Caches
{
	public abstract class EntitiesCache<TCacheItem>
    {
        protected CacheItemPolicy Policy { get; private set; }
        protected string Prefix { get; private set; }

        protected EntitiesCache()
        {
            Prefix = "Cache_" + typeof(TCacheItem).Name;

            Policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(GlobalConstants.CacheLiveTimeInMinutes)
            };
        }

        public void SetInvalidationCache(string connectionString, string invalidationGetter)
        {
            SqlDependency.Start(connectionString);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(invalidationGetter, conn))
                {
                    command.Notification = null;

                    SqlDependency dep = new SqlDependency();
                    dep.AddCommandDependency(command);

                    SqlChangeMonitor monitor = new SqlChangeMonitor(dep);

                    Policy.ChangeMonitors.Add(monitor);
                }
            }
        }

        public abstract IEnumerable<TCacheItem> Get(string forUser);

        public abstract void Set(string forUser, IEnumerable<TCacheItem> items);
    }
}
