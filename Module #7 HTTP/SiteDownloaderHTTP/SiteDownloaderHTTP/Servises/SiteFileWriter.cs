using System.IO;

namespace SiteDownloaderHTTP.FileWriters
{
    public class SiteFileWriter
    {
        public void WriteToFile(string root, string name, string content, string extension = null)
        {
            var path = Path.Combine(root, name, extension ?? "");
            CreateFile(root, path, content);
        }

        private void CreateFile(string root, string path, string content)
        {
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            if (File.Exists(path))
                File.Delete(path);

            File.AppendAllText(path, content);
        }
    }
}