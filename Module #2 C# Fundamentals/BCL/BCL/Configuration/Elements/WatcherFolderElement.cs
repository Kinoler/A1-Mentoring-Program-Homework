using System;
using System.Configuration;
using System.Collections;

namespace BCL.Configuration.Elements
{
    public class WatcherFolderElement : ConfigurationElement
    {
        [ConfigurationProperty("Path", IsKey = true)]
        public string Path => (string)base["Path"];
    }

}
