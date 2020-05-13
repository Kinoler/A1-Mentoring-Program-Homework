using System.IO;

namespace SiteDownloaderHTTP.FileWriters
{
    public abstract class SiteFileWriter
    {
        public void WriteToFile(string root, string name, string content)
        {
            var path = GetFilePath(root, name);
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

        protected abstract string GetFilePath(string root, string name);
    }
}