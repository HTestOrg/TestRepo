using BusinessObjects;
using System;
using System.Web.Mvc;
using BusinessLogic.Models;
using Synoptek.SessionManagement;
using Synoptek.Enums;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Stripe;
using System.Linq;
using System.Security.Claims;

namespace Synoptek.Controllers
{
    public class UserRegistrationController : ParentController
    {
        #region Common variables    
        string userID = Convert.ToString(SessionController.UserSession.UserId);
        #endregion

        #region Get user registration data with roles
        public ActionResult Register()
        {
            var userProfileModel = new UserProfileModel();
            Session["ProfileImage"] = null;
            return PartialView("_UserRegistration", userProfileModel);
        }
        #endregion

        #region Get discourse popup
        public ActionResult DiscoursePopup()
        {
            return PartialView("_DiscoursePopup");
            //return PartialView("_UserRegistration", userProfileModel);
        }
        #endregion

        #region Save user registration data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterUser(UserProfileModel userProfileModel)
        {
            var serialization = new Serialization();
            var userRegistration = new UserRegistration();
            var HashCriteria = new Hashtable();
            var HashCriteriaUser = new Hashtable();
            var actualCriteria = string.Empty;
            var actualCriteriaUser = string.Empty;
            if (ModelState.IsValid)
            {
                HashCriteria.Add("Email", userProfileModel.Email);
                actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                var emailresult = userRegistration.CheckForUserName(actualCriteria);
                var emailexist = Convert.ToBoolean(serialization.DeSerializeBinary(Convert.ToString(emailresult)));
                if (!emailexist)
                {
                    HashCriteriaUser.Add("Name", userProfileModel.Name);
                    HashCriteriaUser.Add("Email", userProfileModel.Email);
                    HashCriteriaUser.Add("Address", userProfileModel.Address);
                    HashCriteriaUser.Add("CompanyName", userProfileModel.CompanyName);
                    string password = CipherTool.Encrypt(Convert.ToString(userProfileModel.Password));
                    HashCriteriaUser.Add("Password", password);
                    HashCriteriaUser.Add("Gender", userProfileModel.Gender);
                    //Role ID = 4 if for register user
                    HashCriteriaUser.Add("RoleId", 4);
                    actualCriteriaUser = serialization.SerializeBinary((object)HashCriteriaUser);
                    var result = Convert.ToString(userRegistration.SaveUserProfile(actualCriteriaUser));
                    var userID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(emailresult)));
                    Session["ProfileImage"] = null;
                    SharedFunctions sharedFunctions = new SharedFunctions();
                    sharedFunctions.SendEmail(userProfileModel.Email, Convert.ToString(EmailTemplates.Registration), "Pixere : Registered Successfully", userProfileModel);
                    //Auto Login the user after sign up
                    LoginModel loginModel = new LoginModel();
                    loginModel.UserName = userProfileModel.Email;
                    loginModel.Password = userProfileModel.Password;
                    var url = new
                    {
                        Url = loginModel,
                        type = "Url"
                    };
                    return Json(url, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("", "Sorry, user with email id already exists");
                }
            }
            return PartialView("_UserRegistration", userProfileModel);
        }
        #endregion

        #region Update user profile data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUserDetails(UserProfileEditModel userProfileModel)
        {
            if (ModelState.ContainsKey("SubscriptionModel.CardNumber"))
                ModelState["SubscriptionModel.CardNumber"].Errors.Clear();

            var serialization = new Serialization();
            var userRegistration = new UserRegistration();
            var objUserProfileDetails = new List<UserProfileEditModel>();
            var userProfileEditModel = new UserProfileEditModel();
            var subscriptionController = new SubscriptionController();
            var subscriptionModel = userProfileModel.SubscriptionModel; //new SubscriptionModel();
            var HashCriteria = new Hashtable();
            var HashCriteriaPassword = new Hashtable();
            var actualCriteria = string.Empty;
            var actualCriteriaPassword = string.Empty;
            var passwordIfCorrect = false;

            HashCriteria.Add("ID", userProfileModel.ID);
            HashCriteria.Add("Name", userProfileModel.Name);
            HashCriteria.Add("Address", userProfileModel.Address);
            HashCriteria.Add("City", userProfileModel.City);
            HashCriteria.Add("StateName", userProfileModel.StateName);
            HashCriteria.Add("ZipCode", userProfileModel.ZipCode);
            HashCriteria.Add("CompanyName", userProfileModel.CompanyName);
            HashCriteria.Add("LicenceNumber", userProfileModel.LicenceNumber);
            HashCriteria.Add("PhoneNumber", userProfileModel.PhoneNumber);
            HashCriteria.Add("UserID", userID);
            userProfileEditModel.ProfileImage = userProfileModel.ProfileImage;
            userProfileEditModel.PhoneNumber = userProfileModel.PhoneNumber;
            userProfileEditModel.Email = userProfileModel.Email;
            userProfileEditModel.CompanyName = userProfileModel.CompanyName;

            if (subscriptionModel != null)
            {
                subscriptionModel.ExpirationYearList = new List<ExpirationYear>();
                subscriptionModel.CardType = new List<PaymentCardTypes>();
                subscriptionModel.ExpirationMonthList = new List<ExpirationMonth>();
                subscriptionModel.ExpirationYearList = subscriptionController.GetExpirationYear();
                subscriptionModel.CardType = subscriptionController.GetPaymentCardType();
                subscriptionModel.ExpirationMonthList = subscriptionController.GetExpirationMonth();
            }
            
            userProfileEditModel.SubscriptionModel = subscriptionModel;
            ModelState.Remove("SecurityCode");

            if (string.IsNullOrWhiteSpace(userProfileModel.CurrentPassword) && !string.IsNullOrWhiteSpace(userProfileModel.Password) && !string.IsNullOrWhiteSpace(userProfileModel.ConfirmPassword))
            {
                ModelState.AddModelError("CurrentPassword", "Please Enter valid current password");
                return View("UserProfile", userProfileModel);
            }

            if (ModelState.IsValid)
            {
                if (SessionController.UserSession.RoleType == "Admin")
                {
                    actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                    var result = userRegistration.UpdateUserProfile(actualCriteria);
                    long updatedUserID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
                    SaveProfileImage(userProfileModel.ID);
                    TempData["UserSuccess"] = "User details has been modified successfully..!";
                    return View("UserProfile", userProfileModel);
                }
                //Check if entered password is exists or not
                HashCriteriaPassword.Add("UserID", userProfileModel.ID);
                actualCriteriaPassword = serialization.SerializeBinary((object)HashCriteriaPassword);
                var userResult = userRegistration.GetUserSpecificDetails(actualCriteriaPassword);
                objUserProfileDetails = (List<UserProfileEditModel>)(serialization.DeSerializeBinary(Convert.ToString(userResult)));
                var UserProfileDetails = objUserProfileDetails.FirstOrDefault();
                bool isValidPassword = false;
                if (!string.IsNullOrWhiteSpace(userProfileModel.CurrentPassword))
                    isValidPassword = CipherTool.Verify(userProfileModel.CurrentPassword, Convert.ToString(UserProfileDetails.Password));

                passwordIfCorrect = true;
                if (passwordIfCorrect)
                {
                    if (isValidPassword)
                    {
                        string newPassword = CipherTool.Encrypt(Convert.ToString(userProfileModel.Password));
                        HashCriteria.Add("Password", newPassword);
                    }

                    if (!string.IsNullOrWhiteSpace(userProfileModel.CurrentPassword) && !isValidPassword)
                    {
                        ModelState.AddModelError("CurrentPassword", "Current password is not correct, Please Enter valid current password");
                        return View("UserProfile", userProfileModel);
                    }

                    actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                    var result = userRegistration.UpdateUserProfile(actualCriteria);
                    SaveProfileImage(Convert.ToInt64(userProfileModel.ID));   // user id
                    int year = 0;
                    if (userProfileModel.SubscriptionModel != null)
                    {
                        if (userProfileModel.SubscriptionModel.ExpirationYear != 0)
                        {
                            var selectedItem = subscriptionModel.ExpirationYearList.Find(p => p.ID == userProfileModel.SubscriptionModel.ExpirationYear);
                            year = Convert.ToInt32(selectedItem.Year);
                        }
                    }

                    //Update the user Credit card details
                    string customerID = Convert.ToString(SessionController.UserSession.CustomerID);
                    if (customerID != null && customerID != "" && (SessionController.UserSession.IsPaid = true))
                    {
                        try
                        {
                            var customerService = new StripeCustomerService();
                            StripeCustomer stripeCustomer = customerService.Get(customerID);
                            var myCard = new StripeCardUpdateOptions();
                            if (userProfileModel.SubscriptionModel.NameOnCard != null)
                            {
                                myCard.Name = userProfileModel.SubscriptionModel.NameOnCard;
                            }
                            myCard.ExpirationYear = year;
                            myCard.ExpirationMonth = userProfileModel.SubscriptionModel.ExpirationMonth;
                            myCard.AddressState = userProfileModel.SubscriptionModel.State;
                            myCard.AddressCity = userProfileModel.SubscriptionModel.City;
                            myCard.AddressZip = userProfileModel.SubscriptionModel.Zip;
                            myCard.AddressLine1 = userProfileModel.SubscriptionModel.BillingAddress;
                            var cardService = new StripeCardService();
                            StripeCard stripeCard = cardService.Update(customerID, stripeCustomer.DefaultSourceId, myCard); // optional isRecipient
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", ex.Message);
                            return View("UserProfile", userProfileModel);
                        }
                    }
                    TempData["UserSuccess"] = "User details has been modified successfully..!";
                    return RedirectToAction("UserProfile", new
                    {
                        passwordIfCorrect = passwordIfCorrect
                    });
                }
                else
                {
                    passwordIfCorrect = false;
                }
            }

            return View("UserProfile", userProfileModel);
        }
        #endregion

        #region Get user detials
        public ActionResult UserProfile(bool passwordIfCorrect = true)
        {
            var serialization = new Serialization();
            var userRegistration = new UserRegistration();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var objUserProfileDetails = new List<UserProfileEditModel>();
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = userRegistration.GetUserSpecificDetails(actualCriteria);
            objUserProfileDetails = (List<UserProfileEditModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            var UserProfileDetails = objUserProfileDetails.FirstOrDefault();
            //UserProfileDetails.ProfileImage = CheckFileExists(UserProfileDetails.ProfileImage, "ProfileImagePath", Convert.ToString(UserProfileDetails.ID), "UserProfile");
            UserProfileDetails.ProfileImage = CheckFileExistsGender(UserProfileDetails.ProfileImage, UserProfileDetails.Gender, "ProfileImagePath", Convert.ToString(UserProfileDetails.ID), "UserProfile");
            UserProfileDetails.SubscriptionModel = new SubscriptionModel();
            var subscriptionController = new SubscriptionController();
            UserProfileDetails.SubscriptionModel.CardType = subscriptionController.GetPaymentCardType();
            UserProfileDetails.SubscriptionModel.ExpirationYearList = subscriptionController.GetExpirationYear();
            UserProfileDetails.SubscriptionModel.ExpirationMonthList = subscriptionController.GetExpirationMonth();
            Session["ProfileImage"] = null;
            if (!passwordIfCorrect)
            {
                ViewBag.Error = "Please enter correct password";
            }
            else
            {
                //To get the customer credit card information for this user
                string customerID = UserProfileDetails.CustomerID;
                if (customerID != null && customerID != "")
                {
                    var customerService = new StripeCustomerService();
                    StripeCustomer stripeCustomer = customerService.Get(customerID);
                    var cardService = new StripeCardService();
                    StripeCard stripeCard = cardService.Get(customerID, stripeCustomer.DefaultSourceId); // optional isRecipient
                    foreach (var card in UserProfileDetails.SubscriptionModel.CardType)
                    {
                        //stripeCard.Brand = UserProfileDetails.SubscriptionModel.CardType[0].Name;
                        UserProfileDetails.SubscriptionModel.CardType[0].Name = stripeCard.Brand;
                        UserProfileDetails.SubscriptionModel.CardTypeID = Convert.ToInt32(UserProfileDetails.SubscriptionModel.CardType[0].ID);
                    }
                    if (stripeCard.AddressCity != null)
                    {
                        UserProfileDetails.SubscriptionModel.BillingAddress = stripeCard.AddressCity;
                    }
                    UserProfileDetails.SubscriptionModel.ExpirationMonth = stripeCard.ExpirationMonth;
                    UserProfileDetails.SubscriptionModel.ExpirationYear = stripeCard.ExpirationYear;
                    UserProfileDetails.SubscriptionModel.CardTypeID = UserProfileDetails.SubscriptionModel.CardTypeID;
                    UserProfileDetails.SubscriptionModel.CardNumber = "********" + stripeCard.Last4;
                    UserProfileDetails.SubscriptionModel.NameOnCard = stripeCard.Name;
                    UserProfileDetails.SubscriptionModel.BillingAddress = stripeCard.AddressLine1;
                    UserProfileDetails.SubscriptionModel.City = stripeCard.AddressCity;
                    if (stripeCard.AddressState != null)
                    {
                        UserProfileDetails.SubscriptionModel.State = stripeCard.AddressState;
                    }
                    if (stripeCard.AddressZip != null)
                    {
                        UserProfileDetails.SubscriptionModel.Zip = stripeCard.AddressZip;
                    }
                }
            }
            return View(UserProfileDetails);
        }
        #endregion

        #region Save Profile Image
        public string SaveProfileImage(long userID)
        {
            var serialization = new Serialization();
            var userRegistration = new UserRegistration();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var result = string.Empty;
            var destinationPath = string.Empty;
            var ActualImagePath = string.Empty;
            var TempImagePath = System.Configuration.ConfigurationManager.AppSettings["ProfileTempImagePath"];
            TempImagePath = TempImagePath + "/" + userID;
            var ImagePath = System.Configuration.ConfigurationManager.AppSettings["ProfileImagePath"];
            ImagePath = ImagePath + "/" + userID;
            var folderExists = Directory.Exists(Server.MapPath(ImagePath));
            var lstImages = Convert.ToString(Session["ProfileImage"]);
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(ImagePath));
            if (lstImages != null && lstImages != "")
            {
                var extension = lstImages.Substring(lstImages.LastIndexOf('.') + 1);
                var index = Convert.ToString(lstImages).IndexOf(".");
                var TempserverPath = Server.MapPath(TempImagePath + "/" + lstImages);
                var imageName = lstImages.Substring(0, index);
                var filepresentonserver = Server.MapPath(ImagePath + "/" + lstImages);
                var imagepresenttrue = System.IO.File.Exists(filepresentonserver);
                if (!imagepresenttrue)
                {
                    var filepresent = Server.MapPath(ImagePath + "/" + imageName + "_" + userID + "." + extension);
                    var imagepresent = System.IO.File.Exists(filepresent);
                    if (!imagepresent)
                    {
                        ActualImagePath = Server.MapPath(ImagePath + "/" + imageName + "_" + userID + "." + extension);
                        System.IO.File.Copy(TempserverPath, ActualImagePath);
                    }
                    //Update Profile image in database
                    var FileName = imageName + "_" + userID + "." + extension;
                    HashCriteria.Add("ProfileImagePath", FileName);
                    HashCriteria.Add("UserID", userID);
                    actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                    result = userRegistration.UpdateProfileImagePath(actualCriteria);
                    result = Convert.ToString(serialization.DeSerializeBinary(Convert.ToString(result)));
                }
                System.IO.File.Delete(Server.MapPath(TempImagePath + "/" + lstImages));
            }
            return result;
        }
        #endregion

        #region Upload listing image
        [HttpPost]
        public ActionResult UploadProfileImage(string CurrentUserID)
        {
            var TempImagePath = System.Configuration.ConfigurationManager.AppSettings["ProfileTempImagePath"];
            if (SessionController.UserSession.RoleType == "Admin")
                TempImagePath = TempImagePath + "/" + CurrentUserID;
            else
                TempImagePath = TempImagePath + "/" + SessionController.UserSession.UserId;

            var folderExists = Directory.Exists(Server.MapPath(TempImagePath));
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(TempImagePath));

            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];
                var images = Request.Files[file];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    var stream = fileContent.InputStream;
                    var fileName = Path.GetFileName(images.FileName);
                    var path = Path.Combine(Server.MapPath(TempImagePath), fileName);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        stream.CopyTo(fileStream);
                    }
                    Session["ProfileImage"] = fileName;
                }
            }
            return Json("File uploaded successfully");
        }
        #endregion

        #region Cancel Subscription
        public ActionResult CancelSubscription(long userID)
        {
            string customerID = Convert.ToString(SessionController.UserSession.CustomerID);
            var customerService = new StripeCustomerService();
            StripeCustomer stripeCustomer = customerService.Get(customerID);

            var subscriptionID = stripeCustomer.Subscriptions.Data[0].Id;

            var subscriptionService = new StripeSubscriptionService();
            var status = subscriptionService.Cancel(subscriptionID, true); // optional cancelAtPeriodEnd flag

            SessionController.UserSession.IsPaid = false;

            //Delete the customer's card from stripe
            var cardService = new StripeCardService();
            StripeCard stripeCard = cardService.Get(customerID, stripeCustomer.DefaultSourceId);
            StripeDeleted card = cardService.Delete(customerID, stripeCard.Id);

            return Json(status, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UpdateSelectedUser
        public ActionResult UpdateSelectedUser(string selectedUserID)
        {
            var serialization = new Serialization();
            var userRegistration = new UserRegistration();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var User_ID = Convert.ToInt32(CipherTool.DecryptString(selectedUserID, true));
            var objUserProfileDetails = new List<UserProfileEditModel>();
            HashCriteria.Add("ID", User_ID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = userRegistration.GetUserSpecificDetails(actualCriteria);
            objUserProfileDetails = (List<UserProfileEditModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            var UserProfileDetails = objUserProfileDetails.FirstOrDefault();
            var user = (from u in objUserProfileDetails
                        where u.ID == Convert.ToInt32(User_ID)
                        select u).FirstOrDefault();
            user.ProfileImage = CheckFileExists(user.ProfileImage, "ProfileImagePath", Convert.ToString(user.ID));
            return View("UserProfile", user);
        }
        #endregion

        #region Enable/Desable user
        [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin)]
        public ActionResult EnableDesableUser(string selectedUserID, bool isEnabled)
        {
            var serialization = new Serialization();
            var userRegistration = new UserRegistration();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var User_ID = Convert.ToInt64(CipherTool.DecryptString(selectedUserID, true));
            HashCriteria.Add("ID", User_ID);
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("IsEnabled", isEnabled);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = userRegistration.UpdateUserProfile(actualCriteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete user
        [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin)]
        public ActionResult Delete(string selectedUserID)
        {
            var serialization = new Serialization();
            var userRegistration = new UserRegistration();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var User_ID = Convert.ToInt64(CipherTool.DecryptString(selectedUserID, true));
            HashCriteria.Add("ID", User_ID);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var userRegistrationSeervice = new UserRegistration();
            var result = userRegistrationSeervice.DeleteUser(actualCriteria);
            var resID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion 
    }
}
