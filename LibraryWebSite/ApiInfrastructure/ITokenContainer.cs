using System;
namespace LibraryWebSite.ApiInfrastructure
{
    public interface ITokenContainer
    {
        object ApiToken { get; set; }

        object DeviceSessionToken { get; set; }

        void DeleteCookies();

        void DeleteDeviceSessionCookie();

        bool IsTokenAvailable();

        string GetTokenUser();
    }
}
