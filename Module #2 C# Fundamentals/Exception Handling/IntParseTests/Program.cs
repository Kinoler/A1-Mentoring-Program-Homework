using IntParseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntParseTests
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                var writtenValue = Console.ReadLine();
                int parsedValue;
                try
                {
                    parsedValue = IntParse.Parse(writtenValue);
                }
                catch (FormatException)
                {
                    Console.WriteLine("String is not valid");
                    continue;
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Too much digits");
                    continue;
                }

                Console.WriteLine($"Parse successfully - {parsedValue}");
            }
        }
    }
}
