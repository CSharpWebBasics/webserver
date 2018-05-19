using EduWebServer.Server.HTTP.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduWebServer.Server.HTTP
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void AddHeader(HttpHeader header)
        {
            string key = header.Key;

            headers[key] = header;
        }

        public bool ContainsKey(string key)
        {
            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            return headers[key];
        }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, this.headers.Values);
        }
    }
}
