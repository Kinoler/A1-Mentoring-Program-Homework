using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteDownloaderHTTP;
using SiteDownloaderHTTP.SiteLoader;

namespace SiteDownloaderHTTPConsole
{
    class Program
    {
        static async Task Main()
        {
            SiteLoaderConfiguration siteLoaderConfiguration;
            var extensions = new List<string>();
            try
            {
                siteLoaderConfiguration = (SiteLoaderConfiguration)
                    ConfigurationManager.GetSection("SiteLoaderConfiguration");

                foreach (ExtensionElement element in siteLoaderConfiguration.FileExtensions)
                    extensions.Add(element.Extension);
            }
            catch (Exception)
            {
                Console.WriteLine("Something in configuration is wrong.");
                Console.ReadLine();
                return;
            }

            var loaderSettings = new LoaderSettings(
                siteLoaderConfiguration.MaxDeep,
                siteLoaderConfiguration.DomenLimitation,
                extensions,
                siteLoaderConfiguration.ShowStepsInRealTime);

                var siteLoader = new SiteLoader(loaderSettings);

            siteLoader.SiteLoadStarted += (e, message) => Console.WriteLine(message);

            await siteLoader.LoadAsync(
                    siteLoaderConfiguration.SiteAddress, 
                    siteLoaderConfiguration.PathToDirectory);

            Console.WriteLine("Complete load");
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }
    }
}
