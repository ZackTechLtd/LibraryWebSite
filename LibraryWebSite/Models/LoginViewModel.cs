using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebSite.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is Required", AllowEmptyStrings = false)]
        public string Username { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is Required", AllowEmptyStrings = false)]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        
    }
}
