using System;
using System.ComponentModel.DataAnnotations;
using Common.Models.Api;
using LibraryWebSite.Util;

namespace LibraryWebSite.Models
{
    public class LibraryBookStatusViewModel : JQueryDataTableParamModel
    {
        public string LibraryBookStatusCode { get; set; }

        public string LibraryBookCode { get; set; }

        [Display(Name = "Book Title")]
        [Required]
        public string LibraryBookTitle { get; set; }

        public string LibraryUserCode { get; set; }

        [Display(Name = "Lender")]
        [Required]
        public string LibraryUserName { get; set; }

        [Display(Name = "Date Checked Out")]
        [Required]
        public string DateCheckedOut { get; set; }
        
        [Display(Name = "Date Returned")]
        public string DateReturned { get; set; }

        public string StrDateCheckedOut { get; set; }
        public string StrDateReturned { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateModified { get; set; }

        public string ModifiedBy { get; set; }

        public LibraryBookStatusApiModel GetApiModel(LibraryBookApiModel libraryBook, LibraryUserApiModel libraryuser)
        {
            return new LibraryBookStatusApiModel()
            {
                LibraryBookStatusCode = this.LibraryBookStatusCode,
                DateCheckedOut = Extension.ConvertToDateTime(this.DateCheckedOut),
                DateReturned = Extension.ConvertToDateTime(this.DateReturned),
                CreatedBy = this.CreatedBy,
                DateCreated = this.DateCreated,
                ModifiedBy = this.ModifiedBy,
                DateModified = this.DateModified,
                LibraryBook = libraryBook,
                LibraryUser = libraryuser

            };
        }

        public static LibraryBookStatusViewModel CreateLibraryBookStatusViewModel(LibraryBookStatusApiModel apiModel)
        {
            return new LibraryBookStatusViewModel()
            {
                LibraryBookStatusCode = apiModel.LibraryBookStatusCode,
                DateCheckedOut = (apiModel.DateCheckedOut != null) ? apiModel.DateCheckedOut.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                DateReturned = (apiModel.DateReturned != null) ? apiModel.DateReturned.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                StrDateCheckedOut = (apiModel.DateCheckedOut != null) ? apiModel.DateCheckedOut.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                StrDateReturned = (apiModel.DateReturned != null) ? apiModel.DateReturned.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                CreatedBy = apiModel.CreatedBy,
                DateCreated = apiModel.DateCreated,
                ModifiedBy = apiModel.ModifiedBy,
                DateModified = apiModel.DateModified,
                LibraryBookCode = apiModel.LibraryBook?.LibraryBookCode,
                LibraryBookTitle = apiModel.LibraryBook?.Title,
                LibraryUserCode = apiModel.LibraryUser?.LibraryUserCode,
                LibraryUserName = apiModel.LibraryUser?.LibraryUserName 
            };

        }
    }
}
