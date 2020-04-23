using System.Configuration;
using SiteDownloaderHTTP;

namespace SiteDownloaderHTTPConsole
{
    public class SiteLoaderSettingsConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty(nameof(PathToDirectory), IsRequired = true)]
        public string PathToDirectory => (string)base[nameof(PathToDirectory)];

        [ConfigurationProperty(nameof(SiteAddress), IsRequired = true)]
        public string SiteAddress => (string)base[nameof(SiteAddress)];

        [ConfigurationProperty(nameof(MaxDeep), DefaultValue = 0)]
        public int MaxDeep => (int) base[nameof(MaxDeep)];

        [ConfigurationProperty(nameof(DomenLimitation), DefaultValue = DomenLimitation.WithoutLimitation)]
        public DomenLimitation DomenLimitation => (DomenLimitation) base[nameof(DomenLimitation)];

        [ConfigurationProperty(nameof(ShowStepsInRealTime), DefaultValue = true)]
        public bool ShowStepsInRealTime => (bool) base[nameof(ShowStepsInRealTime)];

        [ConfigurationProperty(nameof(Extensions))]
        public ExtensionCollection Extensions => (ExtensionCollection)this[nameof(Extensions)];
    }

    public class ExtensionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new ExtensionElement();

        protected override object GetElementKey(ConfigurationElement element) => ((ExtensionElement) element).Extension.GetHashCode();
    }

    public class ExtensionElement : ConfigurationElement
    {
        [ConfigurationProperty(nameof(Extension), IsRequired = true)]
        public string Extension => (string) this[nameof(Extension)];
    }
}
