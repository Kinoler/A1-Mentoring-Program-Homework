using System;
using System.Collections.Generic;

namespace LogParser
{
    public static class LogParser
    {
        private const string InfoLevel = "INFO";
        private const string DebugLevel = "DEBUG";
        private const string ErrorLevel = "ERROR";

        public static StatisticModel Parse(string source)
        {
            var lines = source.Split('\n');
            var statisticModel = new StatisticModel();
            var errors = new List<string>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                switch (GetLevel(line))
                {
                    case InfoLevel:
                        statisticModel.InfoCount++;
                        break;
                    case DebugLevel:
                        statisticModel.DebugCount++;
                        break;
                    case ErrorLevel:
                        statisticModel.ErrorCount++;
                        errors.Add(line);
                        break;
                    default:
                        continue;
                }
            }

            statisticModel.Errors = errors.ToArray();
            return statisticModel;
        }

        public static string GetLevel(string line)
        {
            var words = line.Split(' ');
            if (words.Length < 3)
                throw new InvalidOperationException("The file has a wrong format.");

            return words[2];
        }
    }
}
