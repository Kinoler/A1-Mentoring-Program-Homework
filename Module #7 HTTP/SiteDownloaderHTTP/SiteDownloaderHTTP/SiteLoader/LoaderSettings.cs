using System.Collections.Generic;

namespace SiteDownloaderHTTP.SiteLoader
{
    public class LoaderSettings
    {
        public static LoaderSettings Default => 
            new LoaderSettings(
                0,
                DomenLimitation.WithoutLimitation,
                null,
                false);
        
        public int Depth { get; }

        public DomenLimitation DomenLimitation { get; }

        public List<string> ExtensionLimitation { get; }

        public bool ShowStateOnRealTime { get; }

        public LoaderSettings(
            int depth, 
            DomenLimitation domenLimitation,
            List<string> extensionLimitation,
            bool showStateOnRealTime)
        {
            Depth = depth;
            DomenLimitation = domenLimitation;
            ExtensionLimitation = extensionLimitation ?? new List<string>();
            ShowStateOnRealTime = showStateOnRealTime;
        }
    }
}
