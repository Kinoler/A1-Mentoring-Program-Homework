using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCL.Configuration.Elements;
using BCL.Configuration.Sections;

namespace BCL
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = (SimpleConfigurationSection)
                ConfigurationManager.GetSection("simpleSection");

            Console.WriteLine("{0} {1} - {2}",
                s.ApplicationName,
                s.WorkTime.StartTime.ToLongTimeString(),
                (s.WorkTime.StartTime + s.WorkTime.Duration).ToLongTimeString());

            foreach (FileElement file in s.Files)
            {
                Console.WriteLine("{0} - {1}", file.FileName, file.FileSize);
            }
        }
    }
}
