using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Enums;
using Visitor.EventHandler;

namespace Visitor
{
    public class FileSystemVisitor
    {
        private readonly Predicate<ElementFoundEventArgs> _filter;
        private readonly IFileSystem _fileSystem;
        private readonly ElementFoundEventArgs _elementFoundEventArgs;

        public event EventHandler<string> StartSearch;
        public event EventHandler<string> FinishSearch;

        public event EventHandler<ElementFoundEventArgs> FileFound;
        public event EventHandler<ElementFoundEventArgs> DirectoryFound;
        public event EventHandler<ElementFoundEventArgs> FilteredFileFound;
        public event EventHandler<ElementFoundEventArgs> FilteredDirectoryFound;

        public FileSystemVisitor() : this(new FileSystem(), null) { }

        public FileSystemVisitor(IFileSystem fileSystem) : this(fileSystem, null) { }

        public FileSystemVisitor(Predicate<ElementFoundEventArgs> filter) : this(new FileSystem(), filter) { }

        public FileSystemVisitor(IFileSystem fileSystem, Predicate<ElementFoundEventArgs> filter)
        {
            _fileSystem = fileSystem ?? new FileSystem();
            _filter = filter ?? (st => true);
            _elementFoundEventArgs = new ElementFoundEventArgs();
        }

        public IEnumerable<string> ElementsByPath(string path)
        {
            if (!_fileSystem.Directory.Exists(path))
            {
                throw new ArgumentException($"The folder path {path} does not exist");
            }

            OnStartSearch(path);

            foreach (var element in TraverseFolder(path))
            {
                yield return element;
            }

            OnFinishSearch(path);
        }

        protected virtual void OnStartSearch(string path)
        {
            StartSearch?.Invoke(this, path);
        }

        protected virtual void OnFinishSearch(string path)
        {
            FinishSearch?.Invoke(this, path);
        }

        protected virtual void OnElementFound(EventHandler<ElementFoundEventArgs> elementFound)
        {
            elementFound?.Invoke(this, _elementFoundEventArgs);
        }

        protected virtual void OnFilteredElementFound(EventHandler<ElementFoundEventArgs> filteredElementFound)
        {
            filteredElementFound?.Invoke(this, _elementFoundEventArgs);
        }

        private IEnumerable<string> TraverseFolder(string path)
        {

            foreach (var directory in TraverseForSpecificElement(
                _fileSystem.Directory.GetDirectories(path), 
                DirectoryFound, 
                FilteredDirectoryFound, 
                ElementType.Directory))
            {
                yield return directory;

                foreach (var element in TraverseFolder(directory))
                {
                    yield return element;
                }
            }

            foreach (var file in TraverseForSpecificElement(
                _fileSystem.Directory.GetFiles(path), 
                FileFound, 
                FilteredFileFound, 
                ElementType.File))
            {
                yield return file;
            }
        }

        private IEnumerable<string> TraverseForSpecificElement(
            IEnumerable<string> elements,
            EventHandler<ElementFoundEventArgs> elementFound,
            EventHandler<ElementFoundEventArgs> filteredElementFound,
            ElementType elementType)
        {
            foreach (var element in elements)
            {
                _elementFoundEventArgs.Path = element;
                _elementFoundEventArgs.ElementType = elementType;
                _elementFoundEventArgs.SearchSettings = SearchSettings.None;

                OnElementFound(elementFound);

                if (_elementFoundEventArgs.SearchSettings.HasFlag(SearchSettings.StopSearch))
                    yield break;
                if (_elementFoundEventArgs.SearchSettings.HasFlag(SearchSettings.ExcludeCurrentElement))
                    continue;

                if (!_filter.Invoke(_elementFoundEventArgs))
                    continue;

                OnFilteredElementFound(filteredElementFound);

                if (_elementFoundEventArgs.SearchSettings.HasFlag(SearchSettings.StopSearch))
                    yield break;
                if (_elementFoundEventArgs.SearchSettings.HasFlag(SearchSettings.ExcludeCurrentElement))
                    continue;

                yield return element;
            }
        }
    }

}
