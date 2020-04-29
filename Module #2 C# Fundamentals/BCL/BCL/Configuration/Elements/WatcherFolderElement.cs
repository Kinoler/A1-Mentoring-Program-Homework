using System;
using System.Configuration;
using System.Collections;

namespace BCL.Configuration.Elements
{
    public class WatcherFolderElement : ConfigurationElement
    {
        [ConfigurationProperty(nameof(Path), IsKey = true, IsRequired = true)]
        public string Path => (string)base[nameof(Path)];
    }

}
