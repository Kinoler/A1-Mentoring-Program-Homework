using System.Configuration;
using SiteDownloaderHTTP;
using SiteDownloaderHTTP.SiteLoader;

namespace SiteDownloaderHTTPConsole
{
    public class SiteLoaderConfiguration : ConfigurationSection
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

        [ConfigurationProperty(nameof(FileExtensions))]
        public ExtensionCollection FileExtensions => (ExtensionCollection)this[nameof(FileExtensions)];
    }
}
