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
                var writtenValue = Console.ReadLine();

                try
                {
                    var firstSymbol = GetFirstSymbol(writtenValue);
                    Console.WriteLine($"{firstSymbol}");
                }
                catch (Exception)
                {
                    Console.WriteLine("You wrote an empty string.");
                }
            }
        }

        private static char GetFirstSymbol(string str)
        {
            Validate(str);
            return str[0];
        }

        public static void Validate(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new Exception("You wrote an empty string.");
            }
        }
    }
}
