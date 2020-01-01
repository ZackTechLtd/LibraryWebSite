using System;
using System.Collections.Generic;
using Common.Models.Api;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebSite.Models
{
    public class UserInfoViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsLibrarian { get; set; }

        //List of roles that a user has
        public IEnumerable<string> RoleList { get; set; }

        //List of claims that a user has
        public IEnumerable<ClaimsApiModel> UserRoles { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }

        
        public Dictionary<string, string> Settings { get; set; }

    }
}
