using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteDownloaderHTTP;

namespace SiteDownloaderHTTPConsole
{
    class Program
    {
        static void Main()
        {
            SiteLoaderSettingsConfigurationSection siteLoaderSettingsConfigurationSection;
            var extensions = new List<string>();
            try
            {
                siteLoaderSettingsConfigurationSection = (SiteLoaderSettingsConfigurationSection)
                    ConfigurationManager.GetSection("SiteLoaderSettingsConfigurationSection");

                foreach (ExtensionElement element in siteLoaderSettingsConfigurationSection.Extensions)
                    extensions.Add(element.Extension);
            }
            catch (Exception)
            {
                Console.WriteLine("Something in configuration is wrong.");
                Console.ReadLine();
                return;
            }

            var loaderSettings = new LoaderSettings(
                siteLoaderSettingsConfigurationSection.MaxDeep,
                siteLoaderSettingsConfigurationSection.DomenLimitation,
                extensions,
                siteLoaderSettingsConfigurationSection.ShowStepsInRealTime);

                var siteLoader = new SiteLoader(loaderSettings);

            siteLoader.SiteLoadStarted += (e, message) => Console.WriteLine(message);

            siteLoader
                .Load(
                    siteLoaderSettingsConfigurationSection.SiteAddress, 
                    siteLoaderSettingsConfigurationSection.PathToDirectory)
                .ContinueWith(task => Console.WriteLine("Complete load"));

            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }
    }
}
