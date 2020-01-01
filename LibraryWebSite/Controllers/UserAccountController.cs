using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebSite.ApiInfrastructure;
using LibraryWebSite.ApiInfrastructure.Client;
using LibraryWebSite.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryWebSite.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly ITokenContainer _tokenContainer;
        private readonly ILoginClient _loginClient;

        public UserAccountController(ITokenContainer tokenContainer, ILoginClient loginClient)
        {
            _tokenContainer = tokenContainer;
            _loginClient = loginClient;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            
            try
            {


                var loginSuccess = await PerformLoginActions(model.Username, model.Password);
                if (loginSuccess.StatusIsSuccessful)
                {
                    //AppUserCache.LoadUserCache(_accountClient,_cache, model.Username);
                    //await _userCache.GetUserInformation(true);
                   
                    return RedirectToAction("Index", "Home");
                }

                ModelState.Clear();
                if (loginSuccess.ErrorState != null)
                {
                    foreach (var item in loginSuccess.ErrorState.ModelState)
                    {
                        foreach (string val in item.Value)
                        {
                            ModelState.AddModelError("", val);
                        }

                    }
                    //ModelState.AddModelError("", "Not Licensed"); //  .AddModelError("", loginSuccess.ErrorState. "The username or password is incorrect");
                }
                else
                {
                    ModelState.AddModelError("", "Username Or Password Incorrect");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }


            return View(model);
        }

        private async Task<TokenResponse> PerformLoginActions(string email, string password)
        {
            var response = await _loginClient.Login(email, password);
            if (response.StatusIsSuccessful)
            {
                _tokenContainer.ApiToken = response.Data;
            }

            return response; //.StatusIsSuccessful;
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            //add call to api to logout
            BoolResponse reponse = await _loginClient.LogOff();
            if (reponse.StatusIsSuccessful)
            {
                _tokenContainer.DeleteCookies();
                return RedirectToAction("Login");
            }

            return Redirect(Request.Headers["Referer"].ToString());
            //return RedirectToRoute("LogIn");
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            _tokenContainer.DeleteCookies();
            return RedirectToAction("Login");
        }
    }
}
