using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParser
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Please write path to the file.");
                var path = Console.ReadLine();
                if (!File.Exists(path))
                {
                    Console.WriteLine("That is the wrong path.");
                    continue;
                }

                StatisticModel statisticModel;
                try
                {
                    var fileSource = File.ReadAllText(path);
                    statisticModel = LogParser.Parse(fileSource);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                Console.WriteLine($"Info: {statisticModel.InfoCount}");
                Console.WriteLine($"Debug: {statisticModel.DebugCount}");
                Console.WriteLine($"Error: {statisticModel.ErrorCount}");
                Console.WriteLine($"Errors: {(statisticModel.Errors.Length == 0 ? "None" : "")}");

                foreach (var errorLine in  statisticModel.Errors)
                {
                    Console.WriteLine($"   {errorLine}");
                }
            }
        }
    }
}
