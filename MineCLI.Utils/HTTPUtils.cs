using System.IO;
using System.Net;

namespace MineCLI.Utils
{
    public static class HTTPUtils
    {
        public static string GetText(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

        public static void DownloadFile(string url, string fileName)
        {
            using (var client = new WebClient())
            {
                WebRequest.DefaultWebProxy = null;
                client.Proxy = null;
                client.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36");
                client.DownloadFile(url, fileName);
            }
        }
    }
}