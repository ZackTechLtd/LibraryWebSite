using System;
using System.Threading.Tasks;
using LibraryWebSite.ApiInfrastructure;
using LibraryWebSite.ApiInfrastructure.Client;
using LibraryWebSite.Models;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryWebSite.Util
{
    public interface IUserCache
    {
        Task<UserInfoViewModel> GetUserInformation(bool resetCache = false);

        string GetTokenUser();

        Task<string> GetSettingValue(string key);
    }

    public class UserCache : IUserCache
    {
        private const int _cacheTimeOut = 300;
        private const string _userinfoViewModelKey = "userinfoViewModel";
        private readonly IAccountClient _accountClient;
        private readonly IMemoryCache _cache;
        private readonly ITokenContainer _tokenContainer;
        private readonly object _mainDataCacheLock = new object();

        public UserCache(IAccountClient accountClient, IMemoryCache cache, ITokenContainer tokenContainer)
        {
            _accountClient = accountClient;
            _cache = cache;
            _tokenContainer = tokenContainer;
        }

        public async Task<UserInfoViewModel> GetUserInformation(bool resetCache = false)
        {

            UserInfoViewModel retVal = new UserInfoViewModel();
            string username = _tokenContainer.GetTokenUser();

            if (string.IsNullOrEmpty(username))
            {
                return retVal;
            }

            if (resetCache)
            {
                await LoadUserCache(_accountClient, _cache, username);
                if (!_cache.TryGetValue($"{_userinfoViewModelKey}{username}", out retVal))
                {
                    retVal = new UserInfoViewModel();
                }

                return retVal;
            }

            // Look for cache key.
            if (!_cache.TryGetValue($"{_userinfoViewModelKey}{username}", out retVal))
            {
                // Key not in cache, so get data.
                await LoadUserCache(_accountClient, _cache, username);
                if (!_cache.TryGetValue($"{_userinfoViewModelKey}{username}", out retVal))
                {
                    retVal = new UserInfoViewModel();
                }

            }

            return retVal;
        }

        private async Task<UserInfoViewModel> GetUserInfoViewModel(IAccountClient accountClient, string username)
        {
            var model = new UserInfoViewModel();
            var result = await accountClient.GetUserInfo(username);
            if (result.StatusIsSuccessful)
            {
                model.UserName = result.Data.UserName;
                model.Email = result.Data.Email;
            
                model.IsAdmin = result.Data.IsAdmin;
                model.IsLibrarian = result.Data.IsLibrarian;
                
                model.Settings = result.Data.Settings;
                model.RoleList = result.Data.RoleList;
                model.UserRoles = result.Data.UserRoles;
                
            }

            return model;
        }

        public async Task<bool> LoadUserCache(IAccountClient accountClient, IMemoryCache cache, string username)
        {
            //lock (_mainDataCacheLock)
            {
                UserInfoViewModel model = await GetUserInfoViewModel(accountClient, username);

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(_cacheTimeOut));

                // Save data in cache.
                cache.Remove($"{_userinfoViewModelKey}{username}");
                cache.Set($"{_userinfoViewModelKey}{username}", model, cacheEntryOptions);

                return true;
            }
        }

        

        public string GetTokenUser()
        {
            return _tokenContainer.GetTokenUser();
        }

        public async Task<string> GetSettingValue(string key)
        {
            UserInfoViewModel model = await GetUserInformation();
            if (model != null && model.Settings != null)
            {
                if (model.Settings.ContainsKey(key))
                {
                    return model.Settings[key];
                }
            }
            return null;
        }

    }
}
