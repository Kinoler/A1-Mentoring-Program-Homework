using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IntParseLibrary
{
    public class IntParse
    {
        public static int Parse(string str)
        {
            if (str == null)
                throw new ArgumentNullException($"Argument {nameof(str)} can not be null");

            if (ParseNumber(str, out var number))
                throw new FormatException("Invalid string");

            return number;
        }

        private static bool ParseNumber(string str, out int number)
        {
            bool negative = str[0] == '-';
            int start = negative ? 1 : 0;
            int num = 0;
            int offsetDigitInChar = 48;

            for (int i = start; i < str.Length; i++)
            {
                char ch = str[i];

                if (ch >= '0' && ch <= '9')
                {
                    checked
                    {
                        num *= 10;
                        num += (ch - offsetDigitInChar);
                    }
                }
                else
                {
                    number = num;
                    return true;
                }
            }

            number = negative ? -num : num;
            return false;
        }
    }
}
