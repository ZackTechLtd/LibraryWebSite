using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace LibraryWebSite.ApiInfrastructure
{
    public class TokenContainer : ITokenContainer
    {
        private const string ApiTokenKey = "ApiToken";
        private const string DeviceSessionKey = "dsid";
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenContainer(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public object ApiToken
        {
            get
            {
                return _contextAccessor.HttpContext.Session.GetString(ApiTokenKey);
            }
            set
            {

                if (value == null)
                {
                    _contextAccessor.HttpContext.Session.SetString("Expires", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));

                }
                else
                {
                    _contextAccessor.HttpContext.Session.SetString(ApiTokenKey, value.ToString());
                    _contextAccessor.HttpContext.Session.SetString("Expires", DateTime.Now.AddDays(14).ToString("yyyy-MM-dd HH:mm:ss"));
                }

            }
        }

        public object DeviceSessionToken
        {
            get
            {
                return _contextAccessor.HttpContext.Session.GetString(DeviceSessionKey);
            }
            set
            {
                if (value == null)
                {
                    _contextAccessor.HttpContext.Session.SetString("Expires", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    _contextAccessor.HttpContext.Session.SetString(DeviceSessionKey, value.ToString());
                    _contextAccessor.HttpContext.Session.SetString("Expires", DateTime.Now.AddDays(14).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
        }

        public void DeleteCookies()
        {
            ApiToken = null;
            DeviceSessionToken = null;
            _contextAccessor.HttpContext.Session.SetString(ApiTokenKey, string.Empty);
            _contextAccessor.HttpContext.Session.SetString(DeviceSessionKey, string.Empty);
            _contextAccessor.HttpContext.Session.SetString("Expires", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));

        }

        public void DeleteDeviceSessionCookie()
        {
            DeviceSessionToken = null;
            _contextAccessor.HttpContext.Session.SetString(DeviceSessionKey, string.Empty);
            _contextAccessor.HttpContext.Session.SetString("Expires", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public bool IsTokenAvailable()
        {
            if (this.ApiToken == null || string.IsNullOrEmpty(this.ApiToken.ToString()))
                return false;

            var token = new JwtSecurityToken(jwtEncodedString: this.ApiToken.ToString());

            if (token.ValidTo > DateTime.Now.AddSeconds(30))
                return true;

            return false;
        }

        public string GetTokenUser()
        {
            if (this.ApiToken == null || string.IsNullOrEmpty(this.ApiToken.ToString()))
                return null;

            var token = new JwtSecurityToken(jwtEncodedString: this.ApiToken.ToString());

            if (token.ValidTo > DateTime.Now.AddSeconds(30))
                return token.Subject;
            else
                return null;

        }
    }
}
