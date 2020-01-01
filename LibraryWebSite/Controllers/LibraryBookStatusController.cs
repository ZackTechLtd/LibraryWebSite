using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Models;
using Common.Models.Api;
using Common.Util;
using LibraryWebSite.ApiInfrastructure.Client;
using LibraryWebSite.ApiInfrastructure.Responses;
using LibraryWebSite.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryWebSite.Controllers
{
    public class LibraryBookStatusController : BaseController
    {
        private readonly ILibraryBookStatusClient _libraryBookStatusClient;
        private readonly ILibraryUserClient _libraryUserClient;
        private readonly ILibraryBookClient _libraryBookClient;

        public LibraryBookStatusController(ILibraryBookStatusClient libraryBookStatusClient, ILibraryUserClient libraryUserClient, ILibraryBookClient libraryBookClient)
        {
            _libraryBookStatusClient = libraryBookStatusClient;
            _libraryUserClient = libraryUserClient;
            _libraryBookClient = libraryBookClient;
        }

        // GET: /<controller>/
        public IActionResult Index(LibraryBookStatusViewModel viewModel)
        {
            return View(viewModel);
        }

        public async Task<ActionResult> GetLibraryBookStatus(LibraryBookViewModel param)
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

            LibraryBookStatusPagedListResponse response = await _libraryBookStatusClient.GetLibraryBookStatusPaged(filterParameters);

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
                                 c.LibraryBookStatusCode,
                                 c.DateCheckedOut.ToString(),
                                 c.DateReturned.ToString(),
                                 c.LibraryBook.ISBN,
                                 c.LibraryBook.Title,
                                 c.LibraryBook.CopyNumber.ToString(),
                                 c.LibraryUser.LibraryUserName,
                                 c.LibraryUser.Address,
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

        public async Task<JsonResult> GetUserList(string search)
        {
            search += "%";
            APIItemsResponse response = await _libraryUserClient.GetLibraryUsers(search);

            if (!response.StatusIsSuccessful)
            {
                return Json(new { result = false, errorMessage = GetResponseErrorString(response) });
            }

            return Json(new { result = true, lstUsers = response.Data.Results });
        }

        public async Task<JsonResult> GetBookList(string search)
        {
            search += "%";
            APIItemsResponse response = await _libraryBookClient.GetBookList(search);

            if (!response.StatusIsSuccessful)
            {
                return Json(new { result = false, errorMessage = GetResponseErrorString(response) });
            }

            return Json(new { result = true, lstBooks = response.Data.Results });
        }

        public async Task<ActionResult> LibraryBookStatus(string id = null)
        {
            LibraryBookStatusViewModel viewModel = null;
            if (!string.IsNullOrEmpty(id))
            {
                LibraryBookStatusResponse result = await _libraryBookStatusClient.GetLibraryBookStatusByLibraryBookStatusCode(id);
                if (result.StatusIsSuccessful)
                {
                    viewModel = LibraryBookStatusViewModel.CreateLibraryBookStatusViewModel(result.Data);
                }
                else
                {
                    AddResponseErrorsToModelState(result);
                }
            }

            if (viewModel == null)
            {
                viewModel = new LibraryBookStatusViewModel();
            }



            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveLibraryBookStatus(LibraryBookStatusViewModel viewModel)
        {
            (LibraryBookApiModel libraryBook, LibraryUserApiModel libraryuser) = await GetBookAndUser(viewModel.LibraryBookCode, viewModel.LibraryUserCode);

            if (!ModelState.IsValid || libraryBook == null || libraryuser == null)
            {
                return View("LibraryBookStatus", viewModel);
            }

            string currentCode = string.Empty;
            if (string.IsNullOrEmpty(viewModel.LibraryBookStatusCode))
            {
                var insertResponse = await _libraryBookStatusClient.Insert(viewModel.GetApiModel(libraryBook, libraryuser));
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
                currentCode = viewModel.LibraryBookCode;
                var response = await _libraryBookStatusClient.Update(viewModel.GetApiModel(libraryBook, libraryuser));

                if (!response.StatusIsSuccessful)
                {
                    AddResponseErrorsToModelState(response);
                }
                else
                {
                    TempData["SuccessMessage"] = "Record Updated";
                }
            }

            return View("LibraryBookStatus", viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteLibraryBookStatus(string id)
        {
            BoolResponse response = await _libraryBookStatusClient.Delete(id);

            if (!response.StatusIsSuccessful)
            {
                return Json(new { result = false, errorMessage = GetResponseErrorString(response) });
            }

            return Json(new { result = true });
        }

        private async Task<(LibraryBookApiModel libraryBook, LibraryUserApiModel libraryuser )> GetBookAndUser(string libraryBookCode, string libraryUserCode)
        {
            LibraryBookApiModel libraryBook = null;
            LibraryUserApiModel libraryuser = null;
            var (libraryBookResponse, libraryUserResponse) = await TaskEx.WhenAllTwo(_libraryBookClient.GetLibraryBookByLibraryBookCode(libraryBookCode), _libraryUserClient.GetLibraryUserByLibraryUserCode(libraryUserCode));
            if (libraryBookResponse.StatusIsSuccessful)
            {
                libraryBook = libraryBookResponse.Data;
            }
            else
            {
                AddResponseErrorsToModelState(libraryBookResponse);
            }
            if (libraryUserResponse.StatusIsSuccessful)
            {
                libraryuser = libraryUserResponse.Data;
            }
            else
            {
                AddResponseErrorsToModelState(libraryUserResponse);
            }

            return (libraryBook: libraryBook, libraryuser: libraryuser);
        }
    }
}
