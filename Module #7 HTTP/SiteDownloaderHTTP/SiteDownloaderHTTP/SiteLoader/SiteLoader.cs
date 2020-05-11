using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SiteDownloaderHTTP.FileWriters;

namespace SiteDownloaderHTTP.SiteLoader
{
    public class SiteLoader
    {
        public event EventHandler<string> SiteLoadStarted;
        public event EventHandler<string> SiteLoadError;

        private readonly LoaderSettings _loaderSettings;
        private int _currentDepth;
        private Uri _rootUri;

        public SiteLoader(LoaderSettings loaderSettings = null)
        {
            _loaderSettings = loaderSettings ?? LoaderSettings.Default;
        }

        public async Task LoadAsync(string address, string pathToFile)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException(nameof(address));

            if (string.IsNullOrEmpty(pathToFile))
                throw new ArgumentNullException(nameof(pathToFile));

            _rootUri = address.GetUri();
            await LoadSiteAsync(_rootUri, pathToFile);
        }

        private async Task LoadSiteAsync(Uri address, string pathToSaveSite, string name = null)
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

                var content = await GetContentAsync(address);
                var stringContent = await content.ReadAsStringAsync();
                SiteFileWriter siteFileWriter = null;

                switch (content.Headers.ContentType.MediaType)
                {
                    case "text/html":
                    {
                        if (_currentDepth < _loaderSettings.Depth)
                            await LoadInternalSiteAsync(stringContent, pathToSaveSite, address);

                        siteFileWriter = new HtmlFileWriter();
                        break;
                    }
                    case "application/javascript":
                    case "application/x-javascript":
                    {
                        siteFileWriter = new JsFileWriter();
                        break;
                    }
                }

                siteFileWriter?.WriteJsToFile(pathToSaveSite, nameOfSite, stringContent);
            }
            catch (Exception)
            {
                OnSiteLoadError($"Fail to load {address.AbsoluteUri}");
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

        private async Task LoadInternalSiteAsync(string content, string pathToSaveSite, Uri sourceUri)
        {
            foreach (var reference in GetAllReference(content))
            {
                var rootUri = reference.GetUri(sourceUri);
                if (rootUri == null)
                    continue;

                var name = rootUri.GetComponents(UriComponents.Host | UriComponents.Path, UriFormat.SafeUnescaped).Replace('/', '+');
                _currentDepth++;
                await LoadSiteAsync(rootUri, Path.Combine(pathToSaveSite, name), name);
                _currentDepth--;
            }
        }

        private async Task<HttpContent> GetContentAsync(Uri uri)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(uri.AbsoluteUri);
            response.EnsureSuccessStatusCode();
            return response.Content;
        }


        private IEnumerable<string> GetAllReference(string htmlContent)
        {
            HtmlDocument htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(htmlContent);

            foreach (HtmlNode link in htmlSnippet.DocumentNode.SelectNodes("//a[@href]"))
                yield return link.Attributes["href"].Value;

            foreach (HtmlNode link in htmlSnippet.DocumentNode.SelectNodes("//*[@src]"))
                yield return link.Attributes["src"].Value;
        }

        private void LoadSiteStarted(string name)
        {
            SiteLoadStarted?.Invoke(this, name);
        }
        private void OnSiteLoadError(string name)
        {
            SiteLoadError?.Invoke(this, name);
        }
    }
}