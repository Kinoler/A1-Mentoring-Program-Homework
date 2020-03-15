﻿using IntParseLibrary;
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

                try
                {
                    Valid(readLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

                var firstSymbol = readLine[0];
                Console.WriteLine($"{firstSymbol}");
            }
        }

        public static void Valid(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new Exception("You wrote an empty string.");
            }
        }
    }
}
