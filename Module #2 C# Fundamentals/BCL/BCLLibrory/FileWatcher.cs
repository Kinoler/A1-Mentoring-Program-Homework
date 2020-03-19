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
        private readonly List<Rule> _setOfRules;
        private readonly List<string> _watcherFolders;
        private readonly string _defaultFolder;
        private readonly FileWatcherLogger fileWatcherLogger;

        private List<FileSystemWatcher> _fileSystemWatchers;

        public event EventHandler<string> OnLog;

        public FileWatcher(FileWatcherConfiguration fileWatcherConfiguration)
        {
            _setOfRules = fileWatcherConfiguration.SetOfRules;
            _watcherFolders = fileWatcherConfiguration.WatcherFolders;
            _defaultFolder = fileWatcherConfiguration.DefaultFolder;

            fileWatcherLogger = new FileWatcherLogger(this);
            fileWatcherLogger.OnFileEvent += (sender, e) => OnLog?.Invoke(sender, e);
        }

        public void StartWatch()
        {
            _fileSystemWatchers = new List<FileSystemWatcher>();
            foreach (var folder in _watcherFolders)
            {
                var fileSystemWatcher = new FileSystemWatcher(folder);
                fileSystemWatcher.Created += (FileSystemEventHandler)((sender, e) => FileAddedToFolder(e.FullPath, folder));
                fileSystemWatcher.EnableRaisingEvents = true;

                _fileSystemWatchers.Add(fileSystemWatcher);
            }
        }

        private void FileAddedToFolder(string fullPath, string watcherFolder)
        {
            string targetFolder = null;
            var nameConfiguration = OutputNameConfiguration.NoneModification;
            var fileInfo = new FileInfo(fullPath);

            fileWatcherLogger.FileAddedToWatcherFolder(fileInfo.Name, watcherFolder, fileInfo.CreationTime);

            foreach (var rule in _setOfRules)
            {
                if (Regex.IsMatch(fileInfo.Name, rule.Expression))
                {
                    fileWatcherLogger.RoleFound(fileInfo.Name, rule.Expression);

                    nameConfiguration = rule.OutputNameConfiguration;
                    targetFolder = rule.Target;
                    break;
                }
            }

            if (targetFolder == null)
            {
                fileWatcherLogger.RoleNotFound(fileInfo.Name);

                targetFolder = _defaultFolder;
            }

            MoveFileToLocation(fullPath, targetFolder, nameConfiguration);
        }

        private void MoveFileToLocation(string source, string targetLocation, OutputNameConfiguration nameConfiguration)
        {
            var fileInfo = new FileInfo(source);
            var sourceFileName = fileInfo.Name;

            switch (nameConfiguration)
            {
                case OutputNameConfiguration.AddCreationTime:
                    sourceFileName += $"_{fileInfo.CreationTime}";
                    break;
                case OutputNameConfiguration.AddDateMovement:
                    sourceFileName += $"_{DateTime.Now}";
                    break;
                case OutputNameConfiguration.NoneModification:
                    break;
            }

            File.Move(source, $"{targetLocation}\\{sourceFileName}");

            fileWatcherLogger.FileMoved(sourceFileName, targetLocation);
        }
    }
}
