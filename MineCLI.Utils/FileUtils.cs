using System.Net;
using System.IO;
namespace MineCLI.Utils
{
    public static class FileUtils
    {
        public static void CreateFile(string fileName, string content)
        {
            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.Write(content);
            }
        }

        public static void CreateFileIfNotExist(string fileName, string content)
        {
            if (!File.Exists(fileName))
            {
                FileUtils.CreateFile(fileName, content);
            }
        }
    }
}