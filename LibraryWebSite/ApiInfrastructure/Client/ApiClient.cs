using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using LibraryWebSite.Util;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace LibraryWebSite.ApiInfrastructure.Client
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient httpClient;
        private readonly ITokenContainer tokenContainer;

        public ApiClient(IHttpClientAccessor httpClientAccessor, ITokenContainer tokenContainer, IOptions<AppConfiguration> appConfiguration)
        {
            this.httpClient = httpClientAccessor.Client;
            this.tokenContainer = tokenContainer;
            this.httpClient.BaseAddress = new Uri(appConfiguration.Value.ApiUrl);
        }

        public async Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            AddToken();
            using (var content = new FormUrlEncodedContent(values))
            {
                var query = await content.ReadAsStringAsync().ConfigureAwait(false);
                var requestUriWithQuery = string.Concat(requestUri, "?", query);
                var response = await httpClient.GetAsync(requestUriWithQuery).ConfigureAwait(false);
                return response;
            }
        }

        public async Task<HttpResponseMessage> GetFormEncodedContentWithoutToken(string requestUri, params KeyValuePair<string, string>[] values)
        {
            using (var content = new FormUrlEncodedContent(values))
            {
                var query = await content.ReadAsStringAsync().ConfigureAwait(false);
                var requestUriWithQuery = string.Concat(requestUri, "?", query);
                var response = await httpClient.GetAsync(requestUriWithQuery).ConfigureAwait(false);
                return response;
            }
        }

        public async Task<HttpResponseMessage> PostFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            ApiUser inputModel = new ApiUser() { GrantType = values[0].Value, Username = values[1].Value, Password = values[2].Value };

            // Serialize our concrete class into a JSON String
            //var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(values));
            var stringPayload = JsonConvert.SerializeObject(inputModel);

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var httpClient1 = new HttpClient())
            {
                var httpResponse = await httpClient.PostAsync(requestUri, httpContent);
                return httpResponse;
                /*
                HttpResponseMessage httpResponse = null;
                try
                {
                    // Do the actual request and await the response
                    httpResponse = await httpClient1.PostAsync(requestUri, httpContent);
                    return httpResponse;
                    
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return httpResponse;
                }
                */

            }

            //using (var content = new FormUrlEncodedContent(values))
            //{
            //    var response = await httpClient.PostAsync(requestUri, content);
            //    return response;
            //}
        }

        public async Task<HttpResponseMessage> PutFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            using (var content = new FormUrlEncodedContent(values))
            {
                var response = await httpClient.PutAsync(requestUri, content);
                return response;
            }
        }

        public async Task<HttpResponseMessage> PostJsonEncodedContent<T>(string requestUri, T content) where T : ApiModel
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //AddToken();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            if (tokenContainer.ApiToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
            }

            string json = JsonConvert.SerializeObject(content);

            request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.SendAsync(request);


            //var request = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Post,
            //    RequestUri = new Uri(requestUri)
            //};

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
            //request.Content = new StringContent(JsonConvert.SerializeObject(content), System.Text.Encoding.UTF8, "application/json");

            //// Setup client
            //var client = new System.Net.Http.HttpClient();
            //HttpResponseMessage response = await client.SendAsync(request);





            return response;
        }

        public async Task<HttpResponseMessage> PutJsonEncodedContent<T>(string requestUri, T content) where T : ApiModel
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //AddToken();
            //var response = await httpClient.PutAsJsonAsync(requestUri, content);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, requestUri);
            if (tokenContainer.ApiToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
            }

            string json = JsonConvert.SerializeObject(content);

            request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.SendAsync(request);

            return response;
        }


        public async Task<HttpResponseMessage> DeleteContent(string requestUri)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AddToken();
            var response = await httpClient.DeleteAsync(requestUri);
            return response;
        }

        private void AddToken()
        {
            if (tokenContainer.ApiToken != null)
            {
                //HttpCookie tokenCookie = tokenContainer.ApiToken as HttpCookie;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContainer.ApiToken.ToString());
            }
        }
    }

    public class ApiUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string GrantType { get; set; }
    }
}
