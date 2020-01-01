using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Models;
using LibraryWebSite.ApiInfrastructure.Client;
using LibraryWebSite.ApiInfrastructure.Responses;
using LibraryWebSite.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryWebSite.Controllers
{
    public class LibraryBookController : BaseController
    {
        private readonly ILibraryBookClient _libraryBookClient;

        public LibraryBookController(ILibraryBookClient libraryBookClient)
        {
            _libraryBookClient = libraryBookClient;
        }
        // GET: /<controller>/
        public IActionResult Index(LibraryBookViewModel viewModel)
        {
            return View(viewModel);

        }
        
        public async Task<ActionResult> GetLibraryBooks(LibraryBookViewModel param)
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

            LibraryBookPagedListResponse response = await _libraryBookClient.GetLibraryBooksPaged(filterParameters, true);

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
                                 c.LibraryBookCode,
                                 c.ISBN,
                                 c.Author,
                                 c.Title,
                                 c.CopyNumber.ToString(),
                                 c.IsLost.ToString(),
                                 c.IsStolen.ToString(),
                                 //c.CreatedBy,
                                 //c.DateCreated.ToString(),
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

        public async Task<ActionResult> LibraryBook(string id = null)
        {
            LibraryBookViewModel viewModel = null;
            if (!string.IsNullOrEmpty(id))
            {
                LibraryBookResponse result = await _libraryBookClient.GetLibraryBookByLibraryBookCode(id);
                if (result.StatusIsSuccessful)
                {
                    viewModel = LibraryBookViewModel.CreateLibraryBookViewModel(result.Data);
                }
                else
                {
                    AddResponseErrorsToModelState(result);
                }
            }

            if (viewModel == null)
            {
                viewModel = new LibraryBookViewModel();  
            }

            

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveLibraryBook(LibraryBookViewModel viewModel)
        {
            
            if (!ModelState.IsValid)
            {
                return View("LibraryBook", viewModel);
            }

            string currentCode = string.Empty;
            if (string.IsNullOrEmpty(viewModel.LibraryBookCode))
            {
                var insertResponse = await _libraryBookClient.Insert(viewModel.GetApiModel());
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
                var response = await _libraryBookClient.Update(viewModel.GetApiModel());

                if (!response.StatusIsSuccessful)
                {
                    AddResponseErrorsToModelState(response);
                }
                else
                {
                    TempData["SuccessMessage"] = "Record Updated";
                }
            }

            return View("LibraryBook", viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteLibraryBook(string id)
        {
            BoolResponse response = await _libraryBookClient.Delete(id);

            if (!response.StatusIsSuccessful)
            {
                return Json(new { result = false, errorMessage = GetResponseErrorString(response) });
            }

            return Json(new { result = true });
        }

    }
}
