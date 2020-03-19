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
    public class FileWatcher
    {
        private List<Rule> _setOfRules;
        private List<string> _watcherFolders;
        private string _defaultFolder;
        private CultureInfo _cultureInfo;

        private List<FileSystemWatcher> _fileSystemWatchers;

        public FileWatcher(FileWatcherConfiguration fileWatcherConfiguration)
        {
            _setOfRules = fileWatcherConfiguration.SetOfRules;
            _watcherFolders = fileWatcherConfiguration.WatcherFolders;
            _defaultFolder = fileWatcherConfiguration.DefaultFolder;
            _cultureInfo = fileWatcherConfiguration.CultureInfo;
        }

        public void StartWatch()
        {
            _fileSystemWatchers = new List<FileSystemWatcher>();
            foreach (var folder in _watcherFolders)
            {
                var fileSystemWatcher = new FileSystemWatcher(folder);
                fileSystemWatcher.Created += (FileSystemEventHandler)((sender, e) => FileAddedToFolder(e.FullPath, e.Name));
                fileSystemWatcher.EnableRaisingEvents = true;

                _fileSystemWatchers.Add(fileSystemWatcher);
            }
        }

        private void FileAddedToFolder(string fullPath, string fileName)
        {
            string targetFolder = null;
            foreach (var rule in _setOfRules)
            {
                if (Regex.IsMatch(fileName, rule.Expression))
                {
                    targetFolder = rule.Target;
                    break;
                }
            }

            if (targetFolder == null)
            {
                targetFolder = _defaultFolder;
            }

            MoveFileToLocation(fullPath, targetFolder);
        }

        private void MoveFileToLocation(string source, string targetLocation)
        {
            var fileInfo = new FileInfo(source);
            var sourceFileName = fileInfo.Name;

            File.Move(source, $"{targetLocation}\\{sourceFileName}");
        }
    }
}
