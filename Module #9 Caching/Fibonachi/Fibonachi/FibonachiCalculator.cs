using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Threading;
using Fibonachi.Caches;
using StackExchange.Redis;

namespace Fibonachi
{
    public class FibonachiCalculator : IDisposable
    {
        private readonly IFibonachiCache _cache;
        private string _user;

        public FibonachiCalculator(IFibonachiCache cache)
        {
            this._cache = cache;
        }

        public IEnumerable<long> GetCalculated(int n)
        {
            _user = Thread.CurrentPrincipal.Identity.Name;

            var fibonachiCache = _cache.Get(_user);
            if (fibonachiCache == null)
                _cache.Set(_user, new Dictionary<int, long>());

            for (int i = 1; i < n; i++)
                yield return CalculateFibonachi(i);
        }

        private long CalculateFibonachi(int n)
        {
            var fibonachiCache = _cache.Get(_user);
            if (fibonachiCache.ContainsKey(n))
                return fibonachiCache[n];
            
            long calculatedValue = n > 1 ? CalculateFibonachi(n - 2) + CalculateFibonachi(n - 1) : n;

            fibonachiCache.Add(n, calculatedValue);
            _cache.Set(_user, fibonachiCache);
            return calculatedValue;
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }
    }
}
