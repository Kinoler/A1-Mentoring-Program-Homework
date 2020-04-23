using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SiteDownloaderHTTP
{
    public static class UriHelper
    {
        public static Uri GetUri(this string adress, Uri rootUri = null)
        {
            if (string.IsNullOrWhiteSpace(adress))
                return null;

            var root = rootUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);

            if (Uri.TryCreate(adress, UriKind.Absolute, out var uri))
                return uri;

            if (Uri.TryCreate(root + adress, UriKind.Absolute, out var uri2))
                return uri2;

            int indexOfFirstSeparator = adress.IndexOf("/", StringComparison.Ordinal);
            int len = rootUri?.Segments.Length ?? 0;
            var validAdress = adress.Substring(indexOfFirstSeparator + 1);
            root += rootUri?.Segments.Take(len - indexOfFirstSeparator).Aggregate((seed, el) => seed + el);

            if (Uri.TryCreate(root + validAdress, UriKind.Absolute, out var uri3))
                return uri3;

            return null;
        }

        public static async Task<HttpContent> GetContent(this Uri uri)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(uri.AbsoluteUri);
            response.EnsureSuccessStatusCode();
            return response.Content;
        }

        public static IEnumerable<string> GetAllReference(this string htmlContent)
        {
            HtmlDocument htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(htmlContent);

            foreach (HtmlNode link in htmlSnippet.DocumentNode.SelectNodes("//a[@href]"))
                yield return link.Attributes["href"].Value;

            foreach (HtmlNode link in htmlSnippet.DocumentNode.SelectNodes("//*[@src]"))
                yield return link.Attributes["src"].Value;
        }

        public static string GetFileExtension(this Uri uri)
        {
            var fileName = uri.Segments.Last();
            var index = fileName.LastIndexOf('.');
            return index == -1 ? "html" : fileName.Substring(index + 1);
        }
    }
}
