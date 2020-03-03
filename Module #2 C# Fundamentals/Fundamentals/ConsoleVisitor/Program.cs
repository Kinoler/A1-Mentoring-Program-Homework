using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Visitor;

namespace ConsoleVisitor
{
    internal class Program
    {
        private static readonly char _separate = ' ';
        private static readonly int _countSeparate = 3;
        private static string spaces = " ";
        private static string path = @"C:\Projects\A1-Mentoring-Program-Homework";

        private static void Main()
        {
            var systemVisitor = new FileSystemVisitor(found => 
                !(found.Path.Contains(".git") || 
                  found.Path.Contains(".vs") || 
                  found.Path.Contains("packages") || 
                  found.Path.Contains("Variety .NET")));

            systemVisitor.FilteredDirectoryFound += (sender, el) => spaces = new string(_separate, CalculateSpaces(el.Path) * _countSeparate);
            systemVisitor.FilteredFileFound += (sender, el) => spaces = new string(_separate, CalculateSpaces(el.Path) * _countSeparate);

            foreach (var item in systemVisitor.ElementsByPath(path))
            {
                Console.WriteLine(spaces + item.Substring(item.LastIndexOf('\\') + 1));
            }

            Console.Read();
        }

        private static int CalculateSpaces(string pathFound)
        {
            return pathFound.Count(ch => ch == '\\') - path.Count(ch => ch == '\\');
        }

    }
}
