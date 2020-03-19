using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace BCLLibrory
{
    public class FileWatcherConfiguration
    {
        public List<Rule> SetOfRules { get; }
        public List<string> WatcherFolders { get; }
        public string DefaultFolder { get; }
        public CultureInfo CultureInfo { get; }

        public FileWatcherConfiguration(string defaultFolder) :
            this(defaultFolder, CultureInfo.CurrentUICulture)
        { }

        public FileWatcherConfiguration(string defaultFolder, CultureInfo cultureInfo)
        {
            ValidateFolderPath(defaultFolder);

            DefaultFolder = defaultFolder;
            CultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));
            SetOfRules = new List<Rule>();
            WatcherFolders = new List<string>();
        }

        public void AddWatcherFolder(string folder)
        {
            ValidateFolderPath(folder);

            WatcherFolders.Add(folder);
        }

        public void AddRule(Rule rule)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            ValidateRegexExpression(rule.Expression);
            ValidateFolderPath(rule.Target);

            SetOfRules.Add(rule);
        }

        private void ValidateRegexExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentNullException(nameof(expression));

            try
            {
                Regex.Match("", expression);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Not valid", nameof(expression));
            }
        }

        private void ValidateFolderPath(string fullPath)
        {
            if (fullPath == null)
                throw new ArgumentNullException(nameof(fullPath));

            if (!Directory.Exists(fullPath))
                throw new DirectoryNotFoundException($"The path {fullPath} not found");
        }
    }
}
