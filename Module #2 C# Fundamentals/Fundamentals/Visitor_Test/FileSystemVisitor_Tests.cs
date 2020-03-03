using System;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Visitor;
using Visitor.Enums;

namespace Visitor_Test
{
    [TestFixture]
    public class FileSystemVisitor_Tests
    {
        private MockFileSystem fileSystem;

        [SetUp]
        public void SetUp()
        {
            fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\myfile.txt", new MockFileData("Testing is meh.") },
                { @"c:\demo\jQuery.js", new MockFileData("some js") },
                { @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
            });
        }

        [Test]
        public void ElementsByPath_ShouldGetAllDirectoriesAndFiles()
        {
            var systemVisitor = new FileSystemVisitor(fileSystem);

            var actual = systemVisitor.ElementsByPath("c:\\").ToList();

            var expected = new List<string>()
            {
                @"c:\myfile.txt",
                @"c:\demo\jQuery.js",
                @"c:\demo\image.gif",
                @"c:\demo"
            };

            expected.Sort();
            actual.Sort();

            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void ElementsByPath_ShouldGetAllSortedDirectoriesAndFiles()
        {
            var systemVisitor = new FileSystemVisitor(fileSystem, el => el.ElementType != ElementType.File || (el.Path.EndsWith(".txt") || el.Path.EndsWith(".js")));

            var actual = systemVisitor.ElementsByPath("c:\\").ToList();

            var expected = new List<string>()
            {
                @"c:\myfile.txt",
                @"c:\demo\jQuery.js",
                @"c:\demo"
            };

            expected.Sort();
            actual.Sort();

            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void ElementsByPath_ShouldCallSubscribeForFoundeedAndFilteredFiles()
        {
            var systemVisitor = new FileSystemVisitor(fileSystem, el => el.ElementType != ElementType.File || (el.Path.EndsWith(".txt") || el.Path.EndsWith(".js")));

            var actualFileFound = 0;
            var actualFilteredFileFound = 0; 
            var actualDirectoryFound = 0;
            var actualFilteredDirectoryFound = 0;

            systemVisitor.FileFound += (sender, el) => actualFileFound++;
            systemVisitor.FilteredFileFound += (sender, el) => actualFilteredFileFound++;
            systemVisitor.DirectoryFound += (sender, el) => actualDirectoryFound++;
            systemVisitor.FilteredDirectoryFound += (sender, el) => actualFilteredDirectoryFound++;

            systemVisitor.ElementsByPath("c:\\").ToList();

            var expectedFileFound = 3;
            var expectedFilteredFileFound = 2;
            var expectedDirectoryFound = 1;
            var expectedFilteredDirectoryFound = 1;

            Assert.AreEqual(expectedFileFound, actualFileFound);
            Assert.AreEqual(expectedFilteredFileFound, actualFilteredFileFound);
            Assert.AreEqual(expectedDirectoryFound, actualDirectoryFound);
            Assert.AreEqual(expectedFilteredDirectoryFound, actualFilteredDirectoryFound);
        }
    }
}
