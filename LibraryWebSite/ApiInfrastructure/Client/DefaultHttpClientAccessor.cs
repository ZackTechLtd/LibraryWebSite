using System;
using System.Net.Http;

namespace LibraryWebSite.ApiInfrastructure.Client
{
    public class DefaultHttpClientAccessor : IHttpClientAccessor
    {
        public HttpClient Client { get; private set; }


        public DefaultHttpClientAccessor()
        {
            Client = new HttpClient();
        }
    }

    public interface IHttpClientAccessor
    {
        HttpClient Client { get; }
    }
}
