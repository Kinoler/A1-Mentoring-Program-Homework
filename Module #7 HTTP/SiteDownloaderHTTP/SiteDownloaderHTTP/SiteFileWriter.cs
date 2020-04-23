using System.IO;

namespace SiteDownloaderHTTP
{
    public class SiteFileWriter
    {
        public static void WriteHtmlToFile(string root, string name, string content)
        {
            var htmlPath = Path.Combine(root, name + ".html");
            CreateFile(root, htmlPath, content);
        }

        public static void WriteJsToFile(string root, string name, string content)
        {
            var jsPath = Path.Combine(root, name);
            CreateFile(root, jsPath, content);
        }

        public static void CreateFile(string root, string path, string content)
        {
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            if (File.Exists(path))
                File.Delete(path);

            File.AppendAllText(path, content);
        }
    }
}
