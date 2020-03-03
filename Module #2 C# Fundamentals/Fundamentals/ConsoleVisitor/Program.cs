using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Visitor;

namespace ConsoleVisitor
{
    class Program
    {
        private static readonly string _separate = "   ";
        private static string _spaces = "";
        private static string path = @"C:\Projects\A1-Mentoring-Program-Homework";

        static void Main(string[] args)
        {
            var systemVisitor = new FileSystemVisitor(found => !(found.Path.Contains(".git") || found.Path.Contains(".vs") || found.Path.Contains("packages") || found.Path.Contains("Variety .NET")));

            systemVisitor.FilteredDirectoryFound += (sender, el) => _spaces = Enumerable.Repeat(_separate, CalculateSpaces(el.Path)).Aggregate((seed, pathName) => seed + pathName);
            systemVisitor.FilteredFileFound += (sender, el) => _spaces = Enumerable.Repeat(_separate, CalculateSpaces(el.Path)).Aggregate((seed, pathName) => seed + pathName);

            _spaces = " ";
            foreach (var item in systemVisitor.ElementsByPath(path))
            {
                Console.WriteLine(_spaces + item.Substring(item.LastIndexOf('\\') + 1));
            }

            Console.Read();
        }

        static int CalculateSpaces(string pathFound)
        {
            return pathFound.Split('\\').Length - path.Split('\\').Length;
        }

    }
}
