using System.Configuration;
using BCL.Configuration.Collections;
using BCL.Configuration.Elements;

namespace BCL.Configuration.Sections
{
    public class FileWatcherConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty(nameof(Localization), DefaultValue = "En")]
        public string Localization => (string)base[nameof(Localization)];

        [ConfigurationProperty(nameof(DefaultFolder), IsRequired = true)]
        public string DefaultFolder => (string)base[nameof(DefaultFolder)];

        [ConfigurationProperty(nameof(WatcherFolders))]
        public WatcherFolderCollection WatcherFolders => (WatcherFolderCollection)this[nameof(WatcherFolders)];

        [ConfigurationProperty(nameof(Rules))]
        public RuleCollection Rules => (RuleCollection)this[nameof(Rules)];
    }

}
