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
        private List<Rule> _setOfRules;
        private List<string> _watcherFolders;

        public List<Rule> SetOfRules => _setOfRules.ToList();
        public List<string> WatcherFolders => _watcherFolders.ToList();
        public string DefaultFolder { get; }

        public FileWatcherConfiguration(string defaultFolder)
        {
            ValidateFolderPath(defaultFolder);

            DefaultFolder = defaultFolder;
            _setOfRules = new List<Rule>();
            _watcherFolders = new List<string>();
        }

        public void AddWatcherFolder(string folder)
        {
            ValidateFolderPath(folder);

            _watcherFolders.Add(folder);
        }

        public void AddRule(Rule rule)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            ValidateRegexExpression(rule.Expression);
            ValidateFolderPath(rule.Target);

            _setOfRules.Add(rule);
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
