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
        private const char ReplecerInvalidSymbols = '-';

        private readonly List<Rule> _setOfRules;
        private readonly List<string> _watcherFolders;
        private readonly string _defaultFolder;
        private readonly FileWatcherLogger _fileWatcherLogger;

        private List<FileSystemWatcher> _fileSystemWatchers;

        public event EventHandler<string> Log;

        public FileWatcher(FileWatcherSettings fileWatcherSettings)
        {
            _setOfRules = fileWatcherSettings.SetOfRules;
            _watcherFolders = fileWatcherSettings.WatcherFolders;
            _defaultFolder = fileWatcherSettings.DefaultFolder;

            _fileWatcherLogger = new FileWatcherLogger(this);
            _fileWatcherLogger.OnFileEvent += OnLog;
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

        protected virtual void OnLog(object sender, string e)
        {
            Log?.Invoke(sender, e);
        }

        private void FileAddedToFolder(string fullPath, string watcherFolder)
        {
            string targetFolder = null;
            var nameConfiguration = OutputNameConfiguration.NoneModification;
            var fileInfo = new FileInfo(fullPath);

            _fileWatcherLogger.FileAddedToWatcherFolder(fileInfo.Name, watcherFolder, fileInfo.CreationTime);

            foreach (var rule in _setOfRules)
            {
                if (Regex.IsMatch(fileInfo.Name, rule.Expression))
                {
                    _fileWatcherLogger.RuleFound(fileInfo.Name, rule.Expression);

                    nameConfiguration = rule.OutputNameConfiguration;
                    targetFolder = rule.Target;
                    break;
                }
            }

            if (targetFolder == null)
            {
                _fileWatcherLogger.RuleNotFound(fileInfo.Name);

                targetFolder = _defaultFolder;
            }

            MoveFileToLocation(fullPath, targetFolder, nameConfiguration);
        }

        private void MoveFileToLocation(string source, string targetLocation, OutputNameConfiguration nameConfiguration)
        {
            var fileInfo = new FileInfo(source);
            var sourceFileName = Path.GetFileNameWithoutExtension(fileInfo.Name);

            if (nameConfiguration.HasFlag(OutputNameConfiguration.AddCreationTime))
            {
                sourceFileName += $"_{fileInfo.CreationTime}";
            }

            if (nameConfiguration.HasFlag(OutputNameConfiguration.AddSerialNumber))
            {
                sourceFileName += $"_{Directory.GetFiles(fileInfo.DirectoryName).Length}";
            }

            var i = 1;
            var modifiedFileName = sourceFileName;
            while (File.Exists(Path.Combine(targetLocation, modifiedFileName + fileInfo.Extension)))
            {
                modifiedFileName = $"{sourceFileName} ({i++})";
            }

            if (modifiedFileName.Intersect(Path.GetInvalidFileNameChars()).Any())
            {
                modifiedFileName = modifiedFileName
                    .Intersect(Path.GetInvalidFileNameChars())
                    .Aggregate(
                        modifiedFileName, 
                        (seed, invalidChar) => {
                            return seed.Replace(invalidChar, ReplecerInvalidSymbols);
                        });
            }
            
            var path = Path.Combine(targetLocation, modifiedFileName + fileInfo.Extension);
            File.Move(source, path);

            _fileWatcherLogger.FileMoved(modifiedFileName, targetLocation);
        }
    }
}
