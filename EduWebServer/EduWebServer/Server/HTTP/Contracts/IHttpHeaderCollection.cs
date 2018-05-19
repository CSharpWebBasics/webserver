using System;
using System.Collections.Generic;
using System.Text;

namespace EduWebServer.Server.HTTP.Contracts
{
    public interface IHttpHeaderCollection
    {
        void AddHeader(HttpHeader header);

        bool ContainsKey(string key);

        HttpHeader GetHeader(string key);
    }
}
