using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fibonachi;

namespace FibonachiTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var fibonachiCalculator = new FibonachiCalculatorOutprocess();
            foreach (var value in fibonachiCalculator.StartCalculating(100))
            {
                Console.WriteLine(value);
            }

            Console.ReadLine();
        }
    }
}
