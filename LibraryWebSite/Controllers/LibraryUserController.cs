using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Models;
using LibraryWebSite.ApiInfrastructure.Client;
using LibraryWebSite.ApiInfrastructure.Responses;
using LibraryWebSite.Models;
using LibraryWebSite.Util.Constants;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryWebSite.Controllers
{
    public class LibraryUserController : BaseController
    {
        private readonly ILibraryUserClient _libraryUserClient;

        public LibraryUserController(ILibraryUserClient libraryUserClient)
        {
            _libraryUserClient = libraryUserClient;
        }
        // GET: /<controller>/
        public IActionResult Index(LibraryUserViewModel viewModel)
        {
            return View(viewModel);

        }

        public async Task<ActionResult> GetLibraryUsers(LibraryUserViewModel param)
        {
            int sortDirection = 0;
            string strSortDirection = Request.Query["sSortDir_0"]; // asc or desc
            if (strSortDirection == "asc")
                sortDirection = 0;
            else
                sortDirection = 1;

            int sortColumnIndex = Convert.ToInt32(Request.Query["iSortCol_0"]);
            int pageNumber = (param.iDisplayStart / param.iDisplayLength) + 1;
            PagedBase filterParameters = new PagedBase()
            {
                SearchText = param.sSearch,
                PageNum = pageNumber,
                PageSize = param.iDisplayLength,
                OrderBy = sortColumnIndex,
                SortOrder = sortDirection
            };

            LibraryUserPagedListResponse response = await _libraryUserClient.GetLibraryUsersPaged(filterParameters);

            if (response.ResponseCode == HttpStatusCode.Unauthorized)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, Url.Action("Logout", "UserAccount", new { area = "" }));
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }

            IEnumerable<string[]> result = new List<string[]>();
            int TotalDisplayRecords = 0;
            if (response.StatusIsSuccessful)
            {
                if (response.Data.Results != null)
                {
                    result = from c in response.Data.Results
                             select new[] {
                                 c.LibraryUserCode,
                                 c.LibraryUserName,
                                 c.Address,
                                 c.PhoneNumber,
                                 c.MobilePhoneNumber,
                                 c.Email,
                                 c.ModifiedBy,
                                 c.DateModified.ToString()
                             };

                    TotalDisplayRecords = response.Data.Results.Count();
                }
            }
#pragma warning disable IDE0037
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = response.Data.SearchResultCount,
                iTotalDisplayRecords = response.Data.SearchResultCount,
                aaData = result
            });
#pragma warning restore IDE0037
        }

        public async Task<ActionResult> LibraryUser(string id = null)
        {
            LibraryUserViewModel viewModel = null;
            if (!string.IsNullOrEmpty(id))
            {
                LibraryUserResponse result = await _libraryUserClient.GetLibraryUserByLibraryUserCode(id);
                if (result.StatusIsSuccessful)
                {
                    viewModel = LibraryUserViewModel.CreateLibraryUserViewModel(result.Data);
                }
                else
                {
                    AddResponseErrorsToModelState(result);
                }
            }

            if (viewModel == null)
            {
                viewModel = new LibraryUserViewModel();
            }

            viewModel.Counties = Counties.PopulateCountySelectList(viewModel.County);
            viewModel.Countries = Countries.PopulateCountrySelectList(viewModel.Country);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveLibraryUser(LibraryUserViewModel viewModel)
        {
            if (viewModel.CountyIds != null && viewModel.CountyIds.Length > 0) viewModel.County = viewModel.CountyIds[0];
            if (viewModel.CountryIds != null && viewModel.CountryIds.Length > 0) viewModel.Country = viewModel.CountryIds[0];

            if (!ModelState.IsValid)
            {
                viewModel.Counties = Counties.PopulateCountySelectList(viewModel.County);
                viewModel.Countries = Countries.PopulateCountrySelectList(viewModel.Country);
                return View("LibraryUser", viewModel);
            }

            string currentCode = string.Empty;
            if (string.IsNullOrEmpty(viewModel.LibraryUserCode))
            {
                var insertResponse = await _libraryUserClient.Insert(viewModel.GetApiModel());
                if (!insertResponse.StatusIsSuccessful)
                {
                    AddResponseErrorsToModelState(insertResponse);
                }
                else
                {
                    //viewModel.Id = insertResponse.ResponseResult;
                    currentCode = insertResponse.ResponseResult;
                    TempData["SuccessMessage"] = "Record Inserted";
                }
            }
            else
            {
                currentCode = viewModel.LibraryUserCode;
                var response = await _libraryUserClient.Update(viewModel.GetApiModel());

                if (!response.StatusIsSuccessful)
                {
                    AddResponseErrorsToModelState(response);
                }
                else
                {
                    TempData["SuccessMessage"] = "Record Updated";
                }
            }

            viewModel.Counties = Counties.PopulateCountySelectList(viewModel.County);
            viewModel.Countries = Countries.PopulateCountrySelectList(viewModel.Country);

            return View("LibraryUser", viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteLibraryUser(string id)
        {
            BoolResponse response = await _libraryUserClient.Delete(id);

            if (!response.StatusIsSuccessful)
            {
                return Json(new { result = false, errorMessage = GetResponseErrorString(response) });
            }

            return Json(new { result = true });
        }

    }
}
