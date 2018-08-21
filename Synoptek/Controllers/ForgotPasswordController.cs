using BusinessLogic.Models;
using BusinessObjects;
using Synoptek.Enums;
using Synoptek.SessionManagement;
using System;
using System.Collections;
using System.Security.Claims;
using System.Web.Mvc;

namespace Synoptek.Controllers
{
    [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin + "," + Helpers.Constants.UserRoles.Broker + "," + Helpers.Constants.UserRoles.Investor + "," + Helpers.Constants.UserRoles.RegisterUser)]
    public class ForgotPasswordController : ParentController
    {  
        #region Check If User Exists or not
        public ActionResult IfUserExists(string Email)
        {
            var userRegistrationBA = new UserRegistration();
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var IsUserExist = false;
            var result = string.Empty; 
            HashCriteria.Add("Email", Email.Trim());
            actualCriteria = serialization.SerializeBinary((object)HashCriteria); 
            if (!string.IsNullOrWhiteSpace(Email))
                result = userRegistrationBA.CheckForUserName(actualCriteria);

            IsUserExist = Convert.ToBoolean(serialization.DeSerializeBinary(Convert.ToString(result))); 
            if (!IsUserExist)
                return Json("Sorry, User with email id does not exists", JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Send Forgot Password Notification
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SendForgotPasswordNotification(string EmailTo)
        {
            var sharedFunctions = new SharedFunctions();
            var forgotPasswordModel = new ForgotPassword();
            forgotPasswordModel.Email = EmailTo;
            var result = sharedFunctions.SendEmail(EmailTo, Convert.ToString(EmailTemplates.ForgotPassword), "Synoptek : Reset your password", forgotPasswordModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Change password
        public ActionResult ChangePassword(string token = null)
        {
            var loginBA = new Login();
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var changePasswordModel = new ChangePasswordModel();
            var userDetailsModel = new UserDetailsModel();
            if (token != null && token != "")
            {
                HashCriteria.Add("Token", token);
                actualCriteria = serialization.SerializeBinary((object)HashCriteria); 
                var result = loginBA.GetUserDetails(actualCriteria);
                userDetailsModel = (UserDetailsModel)(serialization.DeSerializeBinary(Convert.ToString(result)));  
            }
            changePasswordModel.Email = userDetailsModel.Email;
            return View("ChangePassword", changePasswordModel);
        }
        #endregion

        #region Change Password Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordPost(string Email, string Password)
        {
            var loginBA = new Login();
            var HashCriteria = new Hashtable();
            var serialization = new Serialization();
            var actualCriteria = string.Empty; 
            HashCriteria.Add("Email", Email);
            var password = CipherTool.EncryptString(Convert.ToString(Password), false);
            HashCriteria.Add("Password", password);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = loginBA.ChangePassword(actualCriteria);
            result = Convert.ToString(serialization.DeSerializeBinary(Convert.ToString(result)));
            if (Convert.ToString(Synoptek.SessionController.UserSession.UserId) == null)
            {
                SessionController.UserSession.EmailAddress = null;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
