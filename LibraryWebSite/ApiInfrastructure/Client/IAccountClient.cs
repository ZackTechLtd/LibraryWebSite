using System;
namespace LibraryWebSite.ApiInfrastructure.Client
{
    using System.Threading.Tasks;
    using LibraryWebSite.ApiInfrastructure.Responses;

    public interface IAccountClient
    {
        Task<AccountResponse> GetUserInfo(string username);
    }
}
