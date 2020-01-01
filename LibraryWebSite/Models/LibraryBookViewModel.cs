using System;
using System.ComponentModel.DataAnnotations;
using Common.Models.Api;

namespace LibraryWebSite.Models
{
    public class LibraryBookViewModel : JQueryDataTableParamModel
    { 
        public string LibraryBookCode { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public bool IsStolen { get; set; }
        public bool IsLost { get; set; }
        [Display(Name = "Copy Number")]
        [Required]
        public int CopyNumber { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateModified { get; set; }

        public string ModifiedBy { get; set; }

        public LibraryBookApiModel GetApiModel()
        {
            return new LibraryBookApiModel()
            {
                LibraryBookCode = this.LibraryBookCode,
                ISBN = this.ISBN,
                Title = this.Title,
                Author = this.Author,
                IsStolen = this.IsStolen,
                IsLost = this.IsLost,
                CopyNumber = this.CopyNumber,
                CreatedBy = this.CreatedBy,
                DateCreated = this.DateCreated,
                ModifiedBy = this.ModifiedBy,
                DateModified = this.DateModified 
            };
        }

        public static LibraryBookViewModel CreateLibraryBookViewModel(LibraryBookApiModel apiModel)
        {
            return new LibraryBookViewModel()
            {
                LibraryBookCode = apiModel.LibraryBookCode,
                ISBN = apiModel.ISBN,
                Title = apiModel.Title,
                Author = apiModel.Author,
                IsStolen = apiModel.IsStolen,
                IsLost = apiModel.IsLost,
                CopyNumber = apiModel.CopyNumber,
                CreatedBy = apiModel.CreatedBy,
                DateCreated = apiModel.DateCreated,
                ModifiedBy = apiModel.ModifiedBy,
                DateModified = apiModel.DateModified
            };
        }
    }
}
