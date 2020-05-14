using System.Configuration;

namespace SiteDownloaderHTTPConsole
{
    public class ExtensionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new ExtensionElement();

        protected override object GetElementKey(ConfigurationElement element) => ((ExtensionElement)element).Extension.GetHashCode();
    }
}
