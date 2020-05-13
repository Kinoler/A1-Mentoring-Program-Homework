using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogParser.Models;

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

                Statistic statisticModel;
                try
                {
                    var perser = new Services.LogParser();
                    var fileLines = File.ReadLines(path);

                    statisticModel = perser.Parse(fileLines);
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
