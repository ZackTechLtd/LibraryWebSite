using System;
using LibraryWebSite.ApiInfrastructure;
using LibraryWebSite.ApiInfrastructure.Client;
using LibraryWebSite.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryWebSite
{
    public static class ServicesConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IApiClient, ApiClient>(); // (provider => apiClient);
            services.AddSingleton<ITokenContainer, TokenContainer>();
            services.AddTransient<IAccountClient, AccountClient>();
            services.AddTransient<IUserCache, UserCache>();
            services.AddTransient<ILoginClient,LoginClient>();
            services.AddTransient<ILibraryBookClient, LibraryBookClient>();
            services.AddTransient<ILibraryUserClient, LibraryUserClient>();
            services.AddTransient<ILibraryBookStatusClient, LibraryBookStatusClient>();
        }
    }
}
