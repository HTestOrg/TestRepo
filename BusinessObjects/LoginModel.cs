using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    [Serializable()]
    public class LoginModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public Common common { get; set; }
    }

    [Serializable()]
    public class ChangePasswordModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [Display(Name = "New Password")]
        [StringLength(20, ErrorMessage = "Password length must be minimum 6 characters.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "The Confirm Password field is required.")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Confirm Password does not match with Password.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }

    [Serializable()]
    public class UserDetailsModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}