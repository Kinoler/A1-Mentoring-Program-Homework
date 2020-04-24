using System.Configuration;
using BCL.Configuration.Elements;

namespace BCL.Configuration.Collections
{
    public class WatcherFolderCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new WatcherFolderElement();

        protected override object GetElementKey(ConfigurationElement element) => ((WatcherFolderElement)element).Path.GetHashCode();
    }

}
