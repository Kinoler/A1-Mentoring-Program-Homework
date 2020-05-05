using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CachingSolutionsSamples.Caches;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Managers
{
    public class OrdersManager : ManagerBase<Order>
    {
        public OrdersManager(CacheBase<Order> cache)
            : base(cache)
        {
        }

        protected override DbSet<Order> GetData(Northwind dbContext)
        {
            return dbContext.Orders;
        }
    }
}
