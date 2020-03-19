using System.Configuration;
using BCL.Configuration.Elements;

namespace BCL.Configuration.Collections
{
    public class FileElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new FileElement();

        protected override object GetElementKey(ConfigurationElement element) => ((FileElement)element).FileName;
    }

}
