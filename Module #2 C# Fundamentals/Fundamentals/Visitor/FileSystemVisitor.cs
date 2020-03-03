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
        private readonly Predicate<ElementFound> _filter;
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
        public FileSystemVisitor(Predicate<ElementFound> filter) : this(new FileSystem(), filter) { }
        public FileSystemVisitor(IFileSystem fileSystem, Predicate<ElementFound> filter)
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

            foreach (var directory in TraverseForSpecificElement(_fileSystem.Directory.GetDirectories(path), DirectoryFound, FilteredDirectoryFound, ElementType.Directory))
            {
                yield return directory;

                var elementsEnumerator = TraverseFolder(directory).GetEnumerator();
                while (true)
                {
                    string element = null;
                    try
                    {
                        if (!elementsEnumerator.MoveNext())
                        {
                            break;
                        }
                        element = elementsEnumerator.Current;
                    }
                    catch
                    {
                        elementsEnumerator.Dispose();
                        break;
                    }

                    yield return element;
                }

                elementsEnumerator.Dispose();
            }

            foreach (var file in TraverseForSpecificElement(_fileSystem.Directory.GetFiles(path), FileFound, FilteredFileFound, ElementType.File))
            {
                yield return file;
            }
        }

        private IEnumerable<string> TraverseForSpecificElement(IEnumerable<string> elements, EventHandler<ElementFound> fileFound, EventHandler<ElementFound> filteredFileFound, ElementType elementType)
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

                _elementFound.ElementType = elementType;
                if (!_filter.Invoke(_elementFound))
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

                yield return element;
            }
        }
    }

}
