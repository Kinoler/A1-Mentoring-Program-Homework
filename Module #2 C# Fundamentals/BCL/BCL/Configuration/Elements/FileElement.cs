using System;
using System.Configuration;
using System.Collections;

namespace BCL.Configuration.Elements
{
    public class FileElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true)]
        public string FileName => (string)base["name"];

        [ConfigurationProperty("size")]
        public int FileSize => (int)base["size"];
    }

}
