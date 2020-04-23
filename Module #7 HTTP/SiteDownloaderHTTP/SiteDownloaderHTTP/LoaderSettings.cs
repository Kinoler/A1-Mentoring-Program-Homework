using System;
using System.Collections.Generic;

namespace SiteDownloaderHTTP
{
    public class LoaderSettings
    {
        public int Deep { get; }

        public DomenLimitation DomenLimitation { get; }

        public List<string> ExtensionLimitation { get; }

        public bool ShowStateOnRealTime { get; }

        public LoaderSettings(
            int deep = 0, 
            DomenLimitation domenLimitation = DomenLimitation.WithoutLimitation,
            List<string> extensionLimitation = null,
            bool showStateOnRealTime = false)
        {
            Deep = deep;
            DomenLimitation = domenLimitation;
            ExtensionLimitation = extensionLimitation ?? new List<string>();
            ShowStateOnRealTime = showStateOnRealTime;
        }
    }
}
