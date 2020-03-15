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
        static void Main(string[] args)
        {
            while (true)
            {
                var readLine = Console.ReadLine();
                int i = 0;
                try
                {
                    i = IntParse.Parse(readLine);
                }
                catch (Exception ex)
                {
                    var exText = ex.ToString();
                    Console.WriteLine(exText.Substring(0, exText.IndexOf(':')));
                    Console.WriteLine(ex.Message);
                    continue;
                }

                Console.WriteLine($"Parse succsesfuly - {i}");
            }
        }
    }
}
