using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Models.Api;

namespace LibraryWebSite.ApiInfrastructure.Client
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetFormEncodedContentWithoutToken(string requestUri, params KeyValuePair<string, string>[] values);
        Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
        Task<HttpResponseMessage> PostFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
        Task<HttpResponseMessage> PostJsonEncodedContent<T>(string requestUri, T content) where T : ApiModel;
        Task<HttpResponseMessage> PutJsonEncodedContent<T>(string requestUri, T content) where T : ApiModel;
        Task<HttpResponseMessage> DeleteContent(string requestUri);

    }
}
