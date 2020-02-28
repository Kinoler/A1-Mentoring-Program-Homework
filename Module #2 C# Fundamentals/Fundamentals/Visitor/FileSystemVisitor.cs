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
        private readonly Predicate<string> _filter;
        private readonly IFileSystem _fileSystem;
        private readonly ElementFound _elementFound;

        public event EventHandler<string> StartSearch;
        public event EventHandler<string> FinishSearch;

        public event EventHandler<ElementFound> FileFound;
        public event EventHandler<ElementFound> DirectoryFound;
        public event EventHandler<ElementFound> FilteredFileFound;
        public event EventHandler<ElementFound> FilteredDirectoryFound;

        public FileSystemVisitor() : this(new FileSystem(), null) { }
        public FileSystemVisitor(IFileSystem fileSystem) : this(fileSystem, null) { }
        public FileSystemVisitor(Predicate<string> filter) : this(new FileSystem(), filter) { }
        public FileSystemVisitor(IFileSystem fileSystem, Predicate<string> filter)
        {
            _fileSystem = fileSystem ?? new FileSystem();
            _filter = filter ?? (st => true);
            _elementFound = new ElementFound();
        }

        public IEnumerable<string> ElementsByPath(string path)
        {
            if (!_fileSystem.Directory.Exists(path))
            {
                throw new ArgumentException($"The folder path {path} does not exist");
            }

            StartSearch?.Invoke(this, path);

            foreach (var element in TraverseFolder(path))
            {
                yield return element;
            }

            FinishSearch?.Invoke(this, path);
        }

        private IEnumerable<string> TraverseFolder(string path)
        {
            yield return path;

            foreach (var directory in TraverseForSpecificElement(_fileSystem.Directory.GetDirectories(path), DirectoryFound, FilteredDirectoryFound))
            {
                yield return directory;

                foreach (var element in TraverseFolder(directory))
                {
                    yield return element;
                }
            }

            foreach (var file in TraverseForSpecificElement(_fileSystem.Directory.GetFiles(path), FileFound, FilteredFileFound))
            {
                yield return file;
            }
        }

        private IEnumerable<string> TraverseForSpecificElement(IEnumerable<string> elements, EventHandler<ElementFound> fileFound, EventHandler<ElementFound> filteredFileFound)
        {
            foreach (var element in elements)
            {
                _elementFound.Path = element;
                fileFound?.Invoke(this, _elementFound);

                if ((_elementFound.Flag & SearchSettings.ExcludeCurrentElement) != 0)
                {
                    _elementFound.Flag ^= SearchSettings.ExcludeCurrentElement;
                    continue;
                }

                if ((_elementFound.Flag & SearchSettings.StopSearch) != 0)
                {
                    yield break;
                }

                if (!_filter.Invoke(element))
                {
                    continue;
                }

                _elementFound.Path = element;
                filteredFileFound?.Invoke(this, _elementFound);

                if ((_elementFound.Flag & SearchSettings.ExcludeCurrentElement) != 0)
                {
                    _elementFound.Flag ^= SearchSettings.ExcludeCurrentElement;
                    continue;
                }

                if ((_elementFound.Flag & SearchSettings.StopSearch) != 0)
                {
                    yield break;
                }
            }
        }
    }

}
