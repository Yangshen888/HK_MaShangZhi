using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HaiKan3.Utils
{
    public static class PostXmlMethod
    {
        public static string PostXmla(string url, string strPost)
        {
            var request = WebRequest.Create(url);
            request.Method = "post";
            request.ContentType = "text/xml";
            request.Headers.Add("charset:utf-8");
            var encoding = Encoding.GetEncoding("utf-8");

            if (strPost != null)
            {
                byte[] buffer = encoding.GetBytes(strPost);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
            }
            else
            {
                request.ContentLength = 0;
            }

            using (HttpWebResponse wr = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(wr.GetResponseStream(), encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
