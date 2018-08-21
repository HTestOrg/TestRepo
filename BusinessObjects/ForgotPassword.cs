using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BusinessObjects
{
    [Serializable()]
    public class ForgotPassword
    {
        [Required(ErrorMessage = "The Email Address field is required.")]
        [EmailAddress]
        [StringLength(50)]
        [DisplayName("Email Address")]
        [Remote("IfUserExists", "ForgotPassword", HttpMethod = "POST")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email.")]
        public string Email { get; set; }
    }
}