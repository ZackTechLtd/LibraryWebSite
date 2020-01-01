
namespace LibraryWebSite.ApiInfrastructure.Client
{
    using Common.Models.Api;
    using LibraryWebSite.ApiInfrastructure.Responses;
    using LibraryWebSite.Util;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AccountClient : ClientBase, IAccountClient
    {
        private string UserInfoUrl = "api/user/userinfo";

        public AccountClient(IOptions<AppConfiguration> appConfiguration, IApiClient apiClient) : base(appConfiguration, apiClient)
        {
            UserInfoUrl = appConfiguration.Value.ApiUrl + UserInfoUrl;
        }

        public async Task<AccountResponse> GetUserInfo(string username)
        {
            return await GetJsonDecodedContent<AccountResponse, UserInfoApiModel>(UserInfoUrl, new KeyValuePair<string, string>("username", username)).ConfigureAwait(false);
        }
    }
}
