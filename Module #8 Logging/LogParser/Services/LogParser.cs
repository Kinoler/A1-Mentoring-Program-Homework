using System;
using System.Collections.Generic;

namespace LogParser.Services
{
    public class LogParser
    {
        private const string InfoLevel = "INFO";
        private const string DebugLevel = "DEBUG";
        private const string ErrorLevel = "ERROR";

        private const int MinWordCount = 3; // In right format
        private const int DebugLavelPosition = 2; // In right format
        public Statistic Parse(IEnumerable< string> source)
        {
            var statisticModel = new Statistic();
            var errors = new List<string>();

            foreach (var line in source)
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

        public string GetLevel(string line)
        {
            var words = line.Split(' ');
            if (words.Length < MinWordCount)
                throw new InvalidOperationException("The file has a wrong format.");

            return words[DebugLavelPosition];
        }
    }
}
