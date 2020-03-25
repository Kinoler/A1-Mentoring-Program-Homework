using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            FileWatcherConfigurationSection fileWatcherConfigurationSection;
            try
            {
                fileWatcherConfigurationSection = (FileWatcherConfigurationSection)
                    ConfigurationManager.GetSection("FileWatcherConfigurationSection");
            }
            catch (Exception)
            {
                Console.WriteLine("Something in configuration is wrong.");
                Console.ReadLine();
                return;
            }

            var cultureInfoFromConfiguration = CultureInfo.GetCultureInfo(fileWatcherConfigurationSection.Localization);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfoFromConfiguration;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfoFromConfiguration;

            FileWatcherSettings fileWatcherSettings;
            try
            {
                fileWatcherSettings = new FileWatcherSettings(fileWatcherConfigurationSection.DefaultFolder);

                foreach (WatcherFolderElement folder in fileWatcherConfigurationSection.WatcherFolders)
                {
                    fileWatcherSettings.AddWatcherFolder(folder.Path);
                }

                foreach (RuleElement rule in fileWatcherConfigurationSection.Rules)
                {
                    fileWatcherSettings.AddRule(new Rule(rule.Expression, rule.Target, rule.NameConfiguration));
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Something expression in configuration is wrong.");
                Console.ReadLine();
                return;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Something the directory path in configuration is wrong.");
                Console.ReadLine();
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("Something in the configuration is wrong.");
                Console.ReadLine();
                return;
            }

            var fw = new FileWatcher(fileWatcherSettings);
            fw.Log += (sender, e) => Console.WriteLine(e);
            fw.StartWatch();

            Console.ReadLine();
        }
    }
}
