using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using StackExchange.Redis;

namespace Fibonachi
{
    public class FibonachiCalculatorOutprocess
    {
        private readonly IDatabase _db;
        private readonly string _regionName = "FibonachiCaching";

        public FibonachiCalculatorOutprocess()
        {
            _db = ConnectionMultiplexer.Connect("localhost").GetDatabase();
        }

        public IEnumerable<long> StartCalculating(int n)
        {
            for (int i = 1; i < n; i++)
            {
                yield return CalculateFibonachi(i);
            }
        }

        private long CalculateFibonachi(int n)
        {
            if (!_db.StringGet(n.ToString()).IsNull)
                return (long)_db.StringGet(n.ToString());
            
            long calculatedValue = n > 1 ? CalculateFibonachi(n - 2) + CalculateFibonachi(n - 1) : n;

            _db.StringSet(n.ToString(), calculatedValue);
            return calculatedValue;
        }
    }
}
