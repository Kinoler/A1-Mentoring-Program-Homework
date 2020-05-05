using System.Collections.Generic;

namespace Fibonachi.Caches
{
    public interface IFibonachiCache
    {
        Dictionary<int, long> Get(string forUser);

        void Set(string forUser, Dictionary<int, long> fibonachiCache);
    }
}
