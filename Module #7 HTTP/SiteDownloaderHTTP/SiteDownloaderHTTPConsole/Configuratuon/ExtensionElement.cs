using System.Configuration;

namespace SiteDownloaderHTTPConsole
{
    public class ExtensionElement : ConfigurationElement
    {
        [ConfigurationProperty(nameof(Extension), IsRequired = true)]
        public string Extension => (string)this[nameof(Extension)];
    }
}
