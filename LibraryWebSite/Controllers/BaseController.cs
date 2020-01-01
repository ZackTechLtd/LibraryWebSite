using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebSite.ApiInfrastructure;
using LibraryWebSite.ApiInfrastructure.Client;
using LibraryWebSite.ApiInfrastructure.Responses;
using LibraryWebSite.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Common.Util;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryWebSite.Controllers
{
    [RedirectingAction]
    public class BaseController : Controller
    {
        protected void AddResponseErrorsToModelState(ApiResponse response)
        {
            //Clears success record
            TempData["SuccessMessage"] = null;
            if (response.ErrorState == null)
            {
                return;
            }
            var errors = response.ErrorState.ModelState;
            if (errors == null)
            {
                if (!string.IsNullOrEmpty(response.ResponseResult))
                {
                    ModelState.AddModelError(string.Empty, response.ResponseResult);
                }
                return;
            }
            foreach (var error in errors)
            {
                if (error.Key == string.Empty)
                {
                    // errors that are not bound to any model field get added automatically and they will be added to the ValidationSummary error list
                    ModelState.AddModelError(string.Empty, error.Value[0]);
                }
                else
                {
                    // map the model errors from the API to fields in the website models with the same name
                    foreach (var entry in
                        from entry in ModelState
                        let matchSuffix = string.Concat(".", entry.Key)
                        where error.Key.EndsWith(matchSuffix,StringComparison.OrdinalIgnoreCase)
                        select entry)
                    {
                        ModelState.AddModelError(entry.Key, error.Value[0]);
                    }
                }
            }
        }

        protected string AddResponseErrorsToJsonErrorMessage(ApiResponse response)
        {
            string errorMessage = string.Empty;
            var errors = response.ErrorState.ModelState;
            if (errors == null)
            {
                return null;
            }
            foreach (var error in errors)
            {
                if (error.Key == string.Empty)
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage += ",";
                    }
                    // errors that are not bound to any model field get added automatically and they will be added to the ValidationSummary error list
                    errorMessage += error.Value[0];
                }
                else
                {
                    // map the model errors from the API to fields in the website models with the same name
                    foreach (var entry in
                        from entry in ModelState
                        let matchSuffix = string.Concat(".", entry.Key)
                        where error.Key.EndsWith(matchSuffix, StringComparison.OrdinalIgnoreCase)
                        select entry)
                    {
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            errorMessage += ",";
                        }

                        errorMessage += error.Value[0];
                    }
                }
            }

            return errorMessage;
        }

        protected string GetResponseErrorString(ApiResponse response)
        {
            string result = null;

            var errors = response.ErrorState.ModelState;
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    if (error.Key == string.Empty)
                    {
                        result += error.Value[0] + " ";
                    }
                    else
                    {
                        foreach (var entry in
                        from entry in ModelState
                        let matchSuffix = string.Concat(".", entry.Key)
                        where error.Key.EndsWith(matchSuffix, StringComparison.OrdinalIgnoreCase)
                        select entry)
                        {
                            result += error.Value[0] + " ";
                        }
                    }
                }
            }
            else
            {
                if (response.ResponseCode == System.Net.HttpStatusCode.BadRequest)
                {
                    result += response.ResponseResult.GetNestedString('[', ']').Replace("\"", "");
                }
            }

            return result;
        }

        protected void ClearModelState()
        {
            var modelStateKeys = ModelState.Keys;
            var formKeys = Request.Form.Keys.Cast<string>();
            var allKeys = modelStateKeys.Concat(formKeys).ToList();

            var culture = CultureInfo.CurrentUICulture;

            foreach (var key in allKeys)
            {
                if (ModelState.ContainsKey(key))
                    ModelState[key].Errors.Clear();
            }

            ModelState.Clear();
        }

    }

    public class RedirectingAction : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            Util.IUserCache userCache = context.HttpContext.RequestServices.GetService(typeof(Util.IUserCache)) as Util.IUserCache;

            var result = userCache.GetUserInformation();
            if (string.IsNullOrEmpty(userCache.GetTokenUser()) || !CheckAndRenewToken().Result)
            {
                context.Result = new RedirectToRouteResult(new Microsoft.AspNetCore.Routing.RouteValueDictionary(new
                {
                    controller = "UserAccount",
                    action = "Login",
                    area = ""
                }));
            }
        }

        private async Task<bool> CheckAndRenewToken()
        {
            try
            {
                ILoginClient loginClient = ServiceLocator.Current.GetInstance<ILoginClient>();
                ITokenContainer tokenContainer = ServiceLocator.Current.GetInstance<ITokenContainer>();
                IConfiguration config = ServiceLocator.Current.GetInstance<IConfiguration>();

                if (loginClient == null || tokenContainer == null || config == null)
                {
                    return false;
                }

                int NumberOfMinutes = 10;

                try
                {
                    IConfiguration miscconfig = config.GetSection("Misc");
                    if (miscconfig != null)
                    {
                        string strNumberOfMinutes = miscconfig.GetSection("TimeoutCheck").Value;
                        if (!int.TryParse(strNumberOfMinutes, out NumberOfMinutes))
                        {
                            NumberOfMinutes = 10;
                        }
                    }
                }
                catch
                {
                    NumberOfMinutes = 10;
                }

                JwtSecurityToken token = new JwtSecurityToken(jwtEncodedString: tokenContainer.ApiToken as string);
                if (token == null)
                    return false;

                TimeSpan ts = token.ValidTo - DateTime.Now;
                if (ts.Minutes > NumberOfMinutes)
                {
                    return true;
                }
                else if (ts.Minutes < NumberOfMinutes)
                {
                    TokenResponse response = await loginClient.RenewToken();
                    if (response.StatusIsSuccessful)
                    {
                        tokenContainer.ApiToken = response.Data;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }


            return false;
        }
    }
}
