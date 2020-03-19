using BCLLibrory.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace BCLLibrory
{
    internal class FileWatcherLogger
    {
        private object _sender;

        public event EventHandler<string> OnFileEvent;

        public FileWatcherLogger(object sender)
        {
            _sender = sender;
        }

        public void ExecuteOnFileEvent(string message)
        {
            OnFileEvent?.Invoke(_sender, message);
        }

        public void FileAddedToWatcherFolder(string sourceFileName, string watcherFolder, DateTime creationTime)
        {
            var message = String.Format(LoggerMessages.FileAddedToWatcherFolder, sourceFileName, watcherFolder, creationTime);
            ExecuteOnFileEvent(message);
        }

        public void RoleFound(string sourceFileName, string expression)
        {
            var message = String.Format(LoggerMessages.RoleFound, sourceFileName, expression);
            ExecuteOnFileEvent(message);
        }

        public void RoleNotFound(string sourceFileName)
        {
            var message = String.Format(LoggerMessages.RoleNotFound, sourceFileName);
            ExecuteOnFileEvent(message);
        }

        public void FileMoved(string sourceFileName, string targetLocation)
        {
            var message = String.Format(LoggerMessages.FileMoved, sourceFileName, targetLocation);
            ExecuteOnFileEvent(message);
        }
    }
}
