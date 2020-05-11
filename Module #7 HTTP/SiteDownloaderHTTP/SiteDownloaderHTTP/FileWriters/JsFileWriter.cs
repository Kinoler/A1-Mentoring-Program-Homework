using System.IO;

namespace SiteDownloaderHTTP.FileWriters
{
    public class JsFileWriter : SiteFileWriter
    {
        protected override string GetFilePath(string root, string name)
        {
            return Path.Combine(root, name);
        }
    }
}
