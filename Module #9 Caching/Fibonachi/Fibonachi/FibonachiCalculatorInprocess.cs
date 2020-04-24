using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using StackExchange.Redis;

namespace Fibonachi
{
    public class FibonachiCalculatorInprocess
    {
        private readonly MemoryCache _fibonachiCaching;
        private readonly string _regionName = "FibonachiCaching";

        public FibonachiCalculatorInprocess()
        {
            _fibonachiCaching = new MemoryCache(_regionName);
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
            if (_fibonachiCaching.Contains(n.ToString()))
                return (long)_fibonachiCaching.Get(n.ToString());
            
            long calculatedValue = n > 1 ? CalculateFibonachi(n - 2) + CalculateFibonachi(n - 1) : n;

            _fibonachiCaching.Add(n.ToString(), calculatedValue, DateTime.Now.AddMinutes(10));
            return calculatedValue;
        }
    }
}
