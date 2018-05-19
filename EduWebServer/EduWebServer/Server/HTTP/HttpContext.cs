namespace EduWebServer.Server.HTTP
{
    using System;

    using Contracts;

    public class HttpContext : IHttpContext
    {
        private readonly IHttpRequest request;

        public HttpContext(string reqString)
        {
            if (reqString == null)
            {
                throw new ArgumentNullException($"Request string: {reqString} can't be null..");
            }

            this.request = new HttpRequest(reqString);
        }

        public IHttpRequest Request => this.request;
    }
}
