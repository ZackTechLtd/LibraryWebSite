using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryWebSite.ApiInfrastructure.Responses;
using LibraryWebSite.Util;
using Microsoft.Extensions.Options;

namespace LibraryWebSite.ApiInfrastructure.Client
{
    public class LoginClient : ClientBase, ILoginClient
    {
        private readonly string TokenUri = "api/token";
        private readonly string RenewTokenUri = "api/TokenRenew";

        public LoginClient(IOptions<AppConfiguration> appConfiguration, IApiClient apiClient) : base(appConfiguration, apiClient)
        {
            TokenUri = appConfiguration.Value.ApiUrl + TokenUri;
        }

        public async Task<TokenResponse> Login(string email, string password)
        {
            var response = await ApiClient.PostFormEncodedContent(TokenUri, "grant_type".AsPair("password"),
                "username".AsPair(email), "password".AsPair(password));
            var tokenResponse = await CreateJsonResponse<TokenResponse>(response);
            if (!response.IsSuccessStatusCode)
            {
                if (response.ReasonPhrase != null && response.ReasonPhrase.Equals("Unauthorized", StringComparison.OrdinalIgnoreCase))
                {
                    string[] errMsg = { "Username Or Password Incorrect" };
                    tokenResponse.ErrorState = new ErrorStateResponse
                    {
                        ModelState = new Dictionary<string, string[]>
                        {
                            { "error", errMsg }
                        }
                    };

                    return tokenResponse;

                }

                string content = await response.Content.ReadAsStringAsync();
                ErrorResponceObject erg = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponceObject>(content);
                if (erg != null)
                {
                    tokenResponse.ErrorState = new ErrorStateResponse
                    {
                        ModelState = new Dictionary<string, string[]>
                        {
                            { "error", erg.Error.ToArray()}
                        }
                    };

                    return tokenResponse;
                }

                if (response.ReasonPhrase != null)
                {
                    string[] errMsg = { response.ReasonPhrase };
                    tokenResponse.ErrorState = new ErrorStateResponse
                    {
                        ModelState = new Dictionary<string, string[]>
                        {
                            { "error", errMsg }
                        }
                    };

                    return tokenResponse;
                }

   
                return tokenResponse;
            }

            //var tokenData = await DecodeContent<dynamic>(response);
            var tokenData = await response.Content.ReadAsStringAsync();

            tokenResponse.Data = tokenData;
            return tokenResponse;
        }

        public async Task<BoolResponse> LogOff()
        {
            return await DeleteContent<BoolResponse>(TokenUri /*+ APIConstants.Delete*/ );
        }

        public async Task<TokenResponse> RenewToken()
        {
            return await GetJsonDecodedContent<TokenResponse, string>(RenewTokenUri).ConfigureAwait(false);
        }
    }
}
