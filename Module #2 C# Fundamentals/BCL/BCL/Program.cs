using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCL.Configuration.Elements;
using BCL.Configuration.Sections;
using BCLLibrory;

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

            var g = new FileWatcherConfiguration("E:\\asdf");

            g.AddWatcherFolder("E:\\asdf\\watcher");

            g.AddRule(new Rule(@"\b[M]\w+", "E:\\asdf\\target", OutputNameConfiguration.NoneModification));

            var fw = new FileWatcher(g);
            fw.OnLog += (sender, e) => Console.WriteLine(e);
            fw.StartWatch();

            Console.ReadLine();
        }
    }
}
