using System;
using System.Threading.Tasks;

namespace LibraryWebSite.ApiInfrastructure.Client
{
    public interface ILoginClient
    {
        Task<TokenResponse> Login(string email, string password);
        Task<TokenResponse> RenewToken();
        //Task<BoolResponse> RegisterViewModel viewModel);
        //Task<ChangePasswordResponse> ChangePassword(ChangePasswordViewModel);
        Task<BoolResponse> LogOff();

    }
}
