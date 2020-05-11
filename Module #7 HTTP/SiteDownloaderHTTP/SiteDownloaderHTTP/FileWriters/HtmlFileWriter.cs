using System.IO;

namespace SiteDownloaderHTTP.FileWriters
{
    public class HtmlFileWriter : SiteFileWriter
    {
        protected override string GetFilePath(string root, string name)
        {
            return Path.Combine(root, name + ".html");
        }
    }
}
