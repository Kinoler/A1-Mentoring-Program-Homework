using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fibonachi;
using Fibonachi.Caches;

namespace FibonachiTests
{
    public class Program
    {
        public static void Main()
        {
            MemoryCache();
            //RedisCache();

            Console.ReadLine();
        }

        public static void MemoryCache()
        {
            PrintFibonachi(new FibonachiMemoryCache());
        }

        public static void RedisCache()
        {
            PrintFibonachi(new FibonachiRedisCache("localhost"));
        }

        public static void PrintFibonachi(IFibonachiCache fibonachiCache)
        {
            using (var fibonachiCalculator = new FibonachiCalculator(fibonachiCache))
            {
                for (var i = 0; i < 10; i++)
                {
                    foreach (var value in fibonachiCalculator.GetCalculated(100))
                        Console.WriteLine(value);

                    Thread.Sleep(100);
                }
            }
        }
    }
}
