
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Json;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SiteDownloaderHTTP
{
    public class SiteLoader
    {
        public event EventHandler<string> SiteLoadStarted;

        private readonly LoaderSettings _loaderSettings;
        private int _currentDeep;
        private Uri _rootUri;

        public SiteLoader(LoaderSettings loaderSettings = null)
        {
            _loaderSettings = loaderSettings ?? new LoaderSettings();
        }

        public async Task Load(string address, string pathToFile)
        {
            _rootUri = address.GetUri();
            await LoadSite(_rootUri, pathToFile);
        }

        private async Task LoadSite(Uri address, string pathToSaveSite, string name = null)
        {
            try
            {
                if (!CheckSiteDomenLimitation(address))
                    return;

                if (!CheckFileExtension(address.GetFileExtension()))
                    return;

                var nameOfSite = name ?? address.DnsSafeHost;

                if (_loaderSettings.ShowStateOnRealTime)
                    LoadSiteStarted(nameOfSite);

                var content = await address.GetContent();
                var stringContent = await content.ReadAsStringAsync();

                switch (content.Headers.ContentType.MediaType)
                {
                    case "text/html":
                    {
                        SiteFileWriter.WriteHtmlToFile(pathToSaveSite, nameOfSite, stringContent);

                        if (_currentDeep < _loaderSettings.Deep)
                            await LoadInternalSite(stringContent, pathToSaveSite, address);

                        return;
                    }
                    case "application/javascript":
                    case "application/x-javascript":
                    {
                        SiteFileWriter.WriteJsToFile(pathToSaveSite, nameOfSite, stringContent);
                        return;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private bool CheckSiteDomenLimitation(Uri address)
        {
            switch (_loaderSettings.DomenLimitation)
            {
                case DomenLimitation.WithoutLimitation:
                    return true;

                case DomenLimitation.CurrentDomainOnly:
                    return address.DnsSafeHost == _rootUri.DnsSafeHost;

                case DomenLimitation.CurrentUrlAndBelow:
                    return address.AbsoluteUri.StartsWith(_rootUri.AbsoluteUri);
            }

            return false;
        }

        private bool CheckFileExtension(string extension)
        {
            return extension == "html" || _loaderSettings.ExtensionLimitation.Contains(extension);
        }

        private async Task LoadInternalSite(string content, string pathToSaveSite, Uri sourceUri)
        {
            foreach (var reference in content.GetAllReference())
            {
                var rootUri = reference.GetUri(sourceUri);
                if (rootUri == null)
                    continue;

                var name = rootUri.GetComponents(UriComponents.Host | UriComponents.Path, UriFormat.SafeUnescaped).Replace('/', '+');
                _currentDeep++;
                await LoadSite(rootUri, Path.Combine(pathToSaveSite, name), name);
                _currentDeep--;
            }
        }

        private void LoadSiteStarted(string name)
        {
            SiteLoadStarted?.Invoke(this, name);
        }
    }
}