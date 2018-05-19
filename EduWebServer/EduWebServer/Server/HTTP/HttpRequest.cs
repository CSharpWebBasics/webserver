namespace EduWebServer.Server.HTTP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Enums;
    using Contracts;
    
    public class HttpRequest : IHttpRequest
    {
        private const string HTTP1_STRING = "http/1.1";
        private const string HOST_STRING = "Host";

        public HttpRequest(string requestString)
        {
            this.HeaderCollection = new HttpHeaderCollection();
            this.UrlParameters = new Dictionary<string, string>();
            this.QueryParameters = new Dictionary<string, string>();
            this.FormData = new Dictionary<string, string>();

            this.ParseRequest(requestString);
        }

        public Dictionary<string, string> FormData { get; private set; }

        public HttpHeaderCollection HeaderCollection { get; private set; }

        public string Path { get; private set; }

        public Dictionary<string, string> QueryParameters { get; private set; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, string> UrlParameters { get; private set; }

        public void AddUrlParameters(string key, string value)
        {
            if (key == "searchTerm")
            {
                value = string.Empty;
                this.UrlParameters[key] = value;
                return;
            }

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"UrlParameter key: {nameof(key)} or value: {nameof(value)} is incorect!");
            }

            this.UrlParameters[key] = value;
        }

        private void ParseRequest(string requestString)
        {
            var requestLines = requestString.Split(Environment.NewLine);

            var requestLine = requestLines[0]
                .Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != HTTP1_STRING)
            {
                //throw new BadRequestException("Invalid request line");
            }

            this.RequestMethod = this.ParseRequestMethod(requestLine[0].ToUpper());
            this.Url = requestLine[1];
            this.Path = GetPath(this.Url);
            this.ParseHeaders(requestLines);
            this.ParseParameters();
        }

        private HttpRequestMethod ParseRequestMethod(string method)
        {
            HttpRequestMethod parsedMethod;
            if (!Enum.TryParse(method, true, out parsedMethod))
            {
                //throw new BadRequestException("Invalid request line");
            }

            return parsedMethod;
        }

        private string GetPath(string url)
        {
            return url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries).First();
        }

        private void ParseHeaders(string[] requestLines)
        {
            var endIndex = Array.IndexOf(requestLines, string.Empty);
            for (int i = 1; i < endIndex; i++)
            {
                var headerArgs = requestLines[i]
                    .Split(new[] { ": " }, StringSplitOptions.None);

                if (headerArgs.Length != 2 || string.IsNullOrWhiteSpace(headerArgs[0]) || string.IsNullOrWhiteSpace(headerArgs[1]))
                {
                    //throw new BadRequestException("Invalid request line");
                }

                var header = new HttpHeader(headerArgs[0], headerArgs[1]);
                this.HeaderCollection.AddHeader(header);
            }

            if (!this.HeaderCollection.ContainsKey(HOST_STRING))
            {
                //throw new BadRequestException("Invalid request line");
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }

            var query = this.Url.Split('?').Last();
            this.ParseQuery(query, this.QueryParameters);
        }

        private void ParseQuery(string query, Dictionary<string, string> dict)
        {
            if (!query.Contains("="))
            {
                return;
            }

            var queryParams = query.Split('&');
            foreach (var queryKVP in queryParams)
            {
                var queryPair = queryKVP.Split('=');

                if (queryPair.Length != 2)
                {
                    continue;
                }

                var queryKey = WebUtility.UrlDecode(queryPair[0]);
                var queryValue = WebUtility.UrlDecode(queryPair[1]);

                dict.Add(queryKey, queryValue);
            }
        }
    }
}
