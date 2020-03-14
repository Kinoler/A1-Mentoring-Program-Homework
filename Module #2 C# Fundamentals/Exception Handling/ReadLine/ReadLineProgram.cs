using IntParseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadLine
{
    internal class ReadLineProgram
    {
        public static void Main()
        {
            while (true)
            {
                var readLine = Console.ReadLine();

                if (string.IsNullOrEmpty(readLine))
                {
                    Console.WriteLine("You wrote an empty string.");
                    continue;
                }

                var firstSymbol = readLine[0];
                Console.WriteLine($"{firstSymbol}");
            }
        }
    }
}
