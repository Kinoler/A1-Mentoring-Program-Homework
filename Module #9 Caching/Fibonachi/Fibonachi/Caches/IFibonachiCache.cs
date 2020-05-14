using System;
using System.Collections.Generic;

namespace Fibonachi.Caches
{
    public interface IFibonachiCache : IDisposable
    {
        Dictionary<int, long> Get(string forUser);

        void Set(string forUser, Dictionary<int, long> fibonachiCache);
    }
}
