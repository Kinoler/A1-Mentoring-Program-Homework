using System.Configuration;
using BCL.Configuration.Collections;
using BCL.Configuration.Elements;

namespace BCL.Configuration.Sections
{
    public class FileWatcherConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("Localization")]
        public string Localization => (string)base["Localization"];

        [ConfigurationProperty("DefaultFolder")]
        public string DefaultFolder => (string)base["DefaultFolder"];

        [ConfigurationProperty("WatcherFolders")]
        public WatcherFolderCollection WatcherFolders => (WatcherFolderCollection)this["WatcherFolders"];

        [ConfigurationProperty("Rules")]
        public RuleCollection Rules => (RuleCollection)this["Rules"];

    }

}
