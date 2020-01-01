using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Models.Api;
using LibraryWebSite.ApiInfrastructure.Responses;
using LibraryWebSite.Util;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LibraryWebSite.ApiInfrastructure.Client
{
    public abstract class ClientBase
    {
        protected readonly IApiClient ApiClient;
        private readonly IOptions<AppConfiguration> _appConfiguration;

        public string _apiUrl;

        protected ClientBase(IOptions<AppConfiguration> appConfiguration, IApiClient apiClient)
        {
            ApiClient = apiClient;
            _appConfiguration = appConfiguration;
        }

        public string GetApiUrl()
        {
            return _appConfiguration.Value.ApiUrl + _apiUrl;
        }

        protected async Task<TResponse> GetJsonDecodedContent<TResponse, TContentResponse>(string uri, params KeyValuePair<string, string>[] requestParameters)
            where TResponse : Responses.ApiResponse<TContentResponse>, new()
        {
            using (var apiResponse = await ApiClient.GetFormEncodedContent(uri, requestParameters).ConfigureAwait(false))
            {
                return await DecodeJsonResponse<TResponse, TContentResponse>(apiResponse).ConfigureAwait(false);
            }
        }

        protected async Task<TResponse> GetJsonDecodedContentWithoutToken<TResponse, TContentResponse>(string uri, params KeyValuePair<string, string>[] requestParameters)
           where TResponse : Responses.ApiResponse<TContentResponse>, new()
        {
            using (var apiResponse = await ApiClient.GetFormEncodedContentWithoutToken(uri, requestParameters).ConfigureAwait(false))
            {
                return await DecodeJsonResponse<TResponse, TContentResponse>(apiResponse).ConfigureAwait(false);
            }
        }

        protected async Task<TResponse> PutEncodedContentWithSimpleIntResponse<TResponse, TModel>(string url, TModel model)
            where TModel : ApiModel
            where TResponse : ApiResponse<int>, new()
        {
            using (var apiResponse = await ApiClient.PutJsonEncodedContent(url, model))
            {
                return await DecodeJsonResponse<TResponse, int>(apiResponse);
            }
        }

        protected async Task<TResponse> PutEncodedContentWithSimpleBoolResponse<TResponse, TModel>(string url, TModel model)
            where TModel : ApiModel
            where TResponse : ApiResponse<bool>, new()
        {
            using (var apiResponse = await ApiClient.PutJsonEncodedContent(url, model))
            {
                return await DecodeJsonResponse<TResponse, bool>(apiResponse);
            }
        }

        protected async Task<TResponse> DeleteContent<TResponse>(string url)
            where TResponse : ApiResponse<bool>, new()
        {
            using (var apiResponse = await ApiClient.DeleteContent(url))
            {
                return await DecodeJsonResponse<TResponse, bool>(apiResponse);
            }
        }

        protected static async Task<TContentResponse> DecodeContent<TContentResponse>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TContentResponse>(result); // Json.Decode<TContentResponse>(result);
        }

        protected async Task<TResponse> PostEncodedContentWithSimpleResponse<TResponse, TModel>(string url, TModel model)
            where TModel : ApiModel
            where TResponse : ApiResponse<int>, new()
        {
            using (var apiResponse = await ApiClient.PostJsonEncodedContent(url, model))
            {
                return await DecodeJsonResponse<TResponse, int>(apiResponse);
            }
        }

        protected async Task<TResponse> PostEncodedContentWithStringResponse<TResponse, TModel>(string url, TModel model)
            where TModel : ApiModel
            where TResponse : ApiResponse<string>, new()
        {
            using (var apiResponse = await ApiClient.PostJsonEncodedContent(url, model))
            {
                return await DecodeJsonResponse<TResponse, string>(apiResponse);
            }
        }

        protected async Task<TResponse> PostEncodedContentWithSimpleBoolResponse<TResponse, TModel>(string url, TModel model)
            where TModel : ApiModel
            where TResponse : ApiResponse<bool>, new()
        {
            using (var apiResponse = await ApiClient.PostJsonEncodedContent(url, model))
            {
                return await DecodeJsonResponse<TResponse, bool>(apiResponse);
            }
        }

        protected static async Task<TResponse> CreateJsonResponse<TResponse>(HttpResponseMessage response) where TResponse : ApiResponse, new()
        {
            var clientResponse = new TResponse
            {
                StatusIsSuccessful = response.IsSuccessStatusCode,
                ErrorState = response.IsSuccessStatusCode ? null : await DecodeContent<ErrorStateResponse>(response),
                ResponseCode = response.StatusCode
            };
            if (response.Content != null)
            {
                clientResponse.ResponseResult = await response.Content.ReadAsStringAsync();
            }

            return clientResponse;
        }

        private static async Task<TResponse> DecodeJsonResponse<TResponse, TDecode>(HttpResponseMessage apiResponse) where TResponse : ApiResponse<TDecode>, new()
        {
            var response = await CreateJsonResponse<TResponse>(apiResponse);
            if (!string.IsNullOrEmpty(response.ResponseResult))
            {
                try
                {
                    response.Data = JsonConvert.DeserializeObject<TDecode>(response.ResponseResult);   //Json.Decode<TDecode>(response.ResponseResult);
                }
                catch
                {
                    //Ignore parameterless constructor errors
                }

            }
            return response;
        }
    }
}
