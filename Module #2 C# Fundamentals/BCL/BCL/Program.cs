using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
            var fileWatcherConfigurationSection = (FileWatcherConfigurationSection)
                ConfigurationManager.GetSection("FileWatcherConfigurationSection");

            var cultureInfoFromConfiguration = CultureInfo.GetCultureInfo(fileWatcherConfigurationSection.Localization);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfoFromConfiguration;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfoFromConfiguration;

            var fileWatcherConfiguration = new FileWatcherConfiguration(fileWatcherConfigurationSection.DefaultFolder);

            foreach (WatcherFolderElement folder in fileWatcherConfigurationSection.WatcherFolders)
            {
                fileWatcherConfiguration.AddWatcherFolder(folder.Path);
            }

            foreach (RuleElement rule in fileWatcherConfigurationSection.Rules)
            {
                fileWatcherConfiguration.AddRule(new Rule(rule.Expression, rule.Target, rule.NameConfiguration));
            }

            var fw = new FileWatcher(fileWatcherConfiguration);
            fw.OnLog += (sender, e) => Console.WriteLine(e);
            fw.StartWatch();

            Console.ReadLine();
        }
    }
}
