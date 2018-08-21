using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BusinessObjects
{
    #region User Profile Model
    [Serializable()]
    public class UserProfileModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [DisplayName("Name")]
        [StringLength(100)]
        [RegularExpression(@"^([a-zA-Z_ ]*)$", ErrorMessage = "The Name field must be alphabet.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Email Address field is required.")]
        [EmailAddress]
        [StringLength(50)]
        [DisplayName("Email Address")]
        [Remote("CheckForUserName", "UserRegistration", HttpMethod = "POST", ErrorMessage = "User name already registered.")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email.")]
        public string Email { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Company Name")]
        [StringLength(50)]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [DisplayName("Password")]
        [StringLength(20, ErrorMessage = "Password length must be minimum 6 characters.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "The Confirm Password field is required.")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Confirm Password does not match with Password.")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("I want to")]
        public int RoleId { get; set; }

        public List<UserRole> RoleList { get; set; }

        public bool IsPaid { get; set; }

        public string CustomerID { get; set; }

        public SubscriptionModel SubscriptionModel { get; set; }

        [Required(ErrorMessage = "Please select a gender.")]
        [DisplayName("Please select a gender.")]
        public string Gender { get; set; }


    }
    #endregion

    #region User Profile Edit Model
    [Serializable()]
    public class UserProfileEditModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [DisplayName("Name")]
        [StringLength(100)]
        [RegularExpression(@"^([a-zA-Z_ ]*)$", ErrorMessage = "The Name field must be alphabet.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Email Address field is required.")]
        [EmailAddress]
        //[StringLength(50)]
        [DisplayName("Email")]
        //[Remote("CheckForUserName", "UserRegistration", HttpMethod = "POST", ErrorMessage = "User name already registered.")]
        //[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address field is required.")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [RegularExpression("^[a-zA-Z\\s]+", ErrorMessage = "Numbers and Special Symbols are not allowed.")]
        [DisplayName("City")]
        public string City { get; set; }

        [RegularExpression("^[a-zA-Z\\s]+", ErrorMessage = "Numbers and Special Symbols are not allowed.")]
        [DisplayName("State")]
        public string StateName { get; set; }

        [DisplayName("Zip Code")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")] 
        public int ZipCode { get; set; }

        [DisplayName("Company Name")]
        [StringLength(50)]
        public string CompanyName { get; set; }

        [DisplayName("Licence Number")]
        [StringLength(50)]
        public string LicenceNumber { get; set; }

        [StringLength(10, ErrorMessage = "Phone Number must be minimum 10 numbers.", MinimumLength = 10)]
        [Required(ErrorMessage = "The Phone Number field is required.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter valid phone number.")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public string ProfileImage { get; set; }

        //[Required(ErrorMessage = "The Password field is required.")]
        [DisplayName("New Password")]
        [StringLength(20, ErrorMessage = "Password length must be minimum 6 characters.", MinimumLength = 6)]
        public string Password { get; set; }

        //[Required(ErrorMessage = "The Confirm Password field is required.")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Confirm Password does not match with Password.")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }

        public bool IsPaid { get; set; }

        public bool IsEnabled { get; set; }

        public SubscriptionModel SubscriptionModel { get; set; }

        public string CustomerID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string Gender { get; set; }
    }
    #endregion

    #region Security Question Model
    [Serializable()]
    public class SecurityQuestion
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    #endregion

    #region User Role Model
    [Serializable()]
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    #endregion
}