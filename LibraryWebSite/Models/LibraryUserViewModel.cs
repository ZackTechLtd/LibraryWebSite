using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models.Api;
using LibraryWebSite.Util;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebSite.Models
{
    public class LibraryUserViewModel : JQueryDataTableParamModel
    {
        public string LibraryUserCode { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Telephone")]
        public string PhoneNumber { get; set; }
        [Display(Name = "MobileNumber")]
        public string MobilePhoneNumber { get; set; }

        [Display(Name = "EmailAddress")]
        public string Email { get; set; }
        [Display(Name = "AlternativePhoneNumber")]
        public string AlternativePhoneNumber { get; set; }
        [Display(Name = "AlternativeEmail")]
        public string AlternativeEmail { get; set; }

        [Required]
        [Display(Name = "AddressLine1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "AddressLine2")]
        public string AddressLine2 { get; set; }
        [Display(Name = "AddressLine3")]
        public string AddressLine3 { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "County")]
        public string County { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }

        [NotMapped]
        [Display(Name = "Address")]
        public string Address
        {
            get
            {
                string address = string.Empty;
                string[] addressParts = { AddressLine1, AddressLine2, AddressLine3, City, County, Postcode };
                foreach (string item in addressParts)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;

                    if (!string.IsNullOrEmpty(address))
                        address += ",";

                    address += item;
                }
                return address;
            }
        }

        [Display(Name = "Name")]
        public string LibraryUserName
        {
            get
            {
                if (!string.IsNullOrEmpty(Title))
                    return $"{Title} {Name}";
                else
                    return Name;
            }
        }

        #region GDPR
        [Display(Name = "Informed Date")]
        public string GDPRInformedDate { get; set; }
        [Display(Name = "Informed By")]
        public string GDPRInformedBy { get; set; }
        [Display(Name = "How Informed")]
        public string GDPRHowInformed { get; set; }
        [Display(Name = "Notes")]
        public string GDPRNotes { get; set; }
        [Display(Name = "Informed User By Post")]
        public bool LibraryUserByPost { get; set; }
        public string LibraryUserByPostConsentDate { get; set; }
        [Display(Name = "Informed User By Email")]
        public bool LibraryUserByEmail { get; set; }
        public string LibraryUserByEmailConsentDate { get; set; }
        [Display(Name = "Informed User By Phone")]
        public bool LibraryUserByPhone { get; set; }
        public string LibraryUserByPhoneConsentDate { get; set; }
        [Display(Name = "Informed User By SMS")]
        public bool LibraryUserBySMS { get; set; }
        public string LibraryUserBySMSConsentDate { get; set; }
        #endregion

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateModified { get; set; }

        public string ModifiedBy { get; set; }

        [Display(Name="Counties")]
        public string[] CountyIds { get; set; }
        public IEnumerable<SelectListItem> Counties { get; set; }

        [Display(Name = "Countries")]
        public string[] CountryIds { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }

        public string StrInformedDate { get; set; }
        
        public string StrContactByPostConsentDate { get; set; }
        

        public string StrContactByEmailConsentDate { get; set; }
        

        public string StrContactByPhoneConsentDate { get; set; }
        

        public string StrContactBySMSConsentDate { get; set; }
        

        public LibraryUserApiModel GetApiModel()
        {
            return new LibraryUserApiModel()
            {
                LibraryUserCode = this.LibraryUserCode,
                Title = this.Title,
                Name = this.Name,
                PhoneNumber = this.PhoneNumber,
                MobilePhoneNumber = this.MobilePhoneNumber,
                Email = this.Email,
                AlternativePhoneNumber = this.AlternativePhoneNumber,
                AlternativeEmail = this.AlternativeEmail,
                AddressLine1 = this.AddressLine1,
                AddressLine2 = this.AddressLine2,
                AddressLine3 = this.AddressLine3,
                City = this.City,
                County = this.County,
                Country = this.Country,
                Postcode = this.Postcode,
                GDPRInformedBy = this.GDPRInformedBy,
                GDPRHowInformed = this.GDPRHowInformed,
                GDPRInformedDate = Extension.ConvertToDateTime(this.GDPRInformedDate),
                GDPRNotes = this.GDPRNotes,
                LibraryUserByEmail = this. LibraryUserByEmail,
                LibraryUserByEmailConsentDate = Extension.ConvertToDateTime(this.LibraryUserByEmailConsentDate),
                LibraryUserByPhone = this.LibraryUserByPhone,
                LibraryUserByPhoneConsentDate = Extension.ConvertToDateTime(this.LibraryUserByPhoneConsentDate),
                LibraryUserByPost = this.LibraryUserByPost,
                LibraryUserByPostConsentDate = Extension.ConvertToDateTime(this.LibraryUserByPostConsentDate),
                LibraryUserBySMS = this.LibraryUserBySMS,
                LibraryUserBySMSConsentDate = Extension.ConvertToDateTime(this.LibraryUserBySMSConsentDate), 
                CreatedBy = this.CreatedBy,
                DateCreated = this.DateCreated,
                ModifiedBy = this.ModifiedBy,
                DateModified = this.DateModified
            };
        }

        public static LibraryUserViewModel CreateLibraryUserViewModel(LibraryUserApiModel apiModel)
        {
            return new LibraryUserViewModel()
            {
                LibraryUserCode = apiModel.LibraryUserCode,
                Title = apiModel.Title,
                Name = apiModel.Name,
                PhoneNumber = apiModel.PhoneNumber,
                MobilePhoneNumber = apiModel.MobilePhoneNumber,
                Email = apiModel.Email,
                AlternativePhoneNumber = apiModel.AlternativePhoneNumber,
                AlternativeEmail = apiModel.AlternativeEmail,
                AddressLine1 = apiModel.AddressLine1,
                AddressLine2 = apiModel.AddressLine2,
                AddressLine3 = apiModel.AddressLine3,
                City = apiModel.City,
                County = apiModel.County,
                Country = apiModel.Country,
                Postcode = apiModel.Postcode,
                GDPRInformedBy = apiModel.GDPRInformedBy,
                GDPRHowInformed = apiModel.GDPRHowInformed,
                GDPRInformedDate = (apiModel.GDPRInformedDate != null) ? apiModel.GDPRInformedDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                StrInformedDate = (apiModel.GDPRInformedDate != null) ? apiModel.GDPRInformedDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                GDPRNotes = apiModel.GDPRNotes,
                LibraryUserByEmail = apiModel.LibraryUserByEmail,
                LibraryUserByEmailConsentDate = (apiModel.LibraryUserByEmailConsentDate != null) ? apiModel.LibraryUserByEmailConsentDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                StrContactByEmailConsentDate = (apiModel.LibraryUserByEmailConsentDate != null) ? apiModel.LibraryUserByEmailConsentDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                LibraryUserByPhone = apiModel.LibraryUserByPhone,
                LibraryUserByPhoneConsentDate = (apiModel.LibraryUserByPhoneConsentDate != null) ? apiModel.LibraryUserByPhoneConsentDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                StrContactByPhoneConsentDate = (apiModel.LibraryUserByPhoneConsentDate != null) ? apiModel.LibraryUserByPhoneConsentDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                LibraryUserByPost = apiModel.LibraryUserByPost,
                LibraryUserByPostConsentDate = (apiModel.LibraryUserByPostConsentDate != null) ? apiModel.LibraryUserByPostConsentDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                StrContactByPostConsentDate = (apiModel.LibraryUserByPostConsentDate != null) ? apiModel.LibraryUserByPostConsentDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                LibraryUserBySMS = apiModel.LibraryUserBySMS,
                LibraryUserBySMSConsentDate = (apiModel.LibraryUserBySMSConsentDate != null) ? apiModel.LibraryUserBySMSConsentDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                StrContactBySMSConsentDate = (apiModel.LibraryUserBySMSConsentDate != null) ? apiModel.LibraryUserBySMSConsentDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                CreatedBy = apiModel.CreatedBy,
                DateCreated = apiModel.DateCreated,
                ModifiedBy = apiModel.ModifiedBy,
                DateModified = apiModel.DateModified
            };
        }
    }
}
