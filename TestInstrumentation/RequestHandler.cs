using System;
using System.Net;
using System.IO;

namespace TestInstrumentation
{
    public class RequestHandler
    {
        /**
         * Handles RESTful GET requests.
         * @param url: The URL of the request
         * @returns: A string response
         */
        public string GetRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";

            var content = string.Empty;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }
            return content;
        }
    }
}
