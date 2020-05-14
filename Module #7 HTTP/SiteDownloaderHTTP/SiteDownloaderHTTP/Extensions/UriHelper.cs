using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SiteDownloaderHTTP
{
    internal static class UriHelper
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

        public static string GetFileExtension(this Uri uri)
        {
            var fileName = uri.Segments.Last();
            var extension = Path.GetExtension(fileName);
            return string.IsNullOrEmpty(extension) ? "html" : extension;
        }
    }
}
