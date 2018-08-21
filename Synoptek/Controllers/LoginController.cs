using BusinessLogic.Models;
using BusinessObjects;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Stripe;
using Synoptek.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Web.Mvc;

namespace Synoptek.Controllers
{
    public class LoginController : ParentController
    {
        #region Common variables  
        string userId = Convert.ToString(SessionController.UserSession.UserId);
        #endregion

        #region Get login details
        [HttpGet]
        public ActionResult Login()
        {
            LogOff();
            return PartialView("_Login");

        }
        #endregion

        #region login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel)
        {
            var serialization = new Serialization();
            var userRegistration = new UserRegistration();
            var loginBA = new Login();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            if (ModelState.IsValid)
            {
                HashCriteria.Add("UserName", loginModel.UserName);
                actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                var result = loginBA.ValidateLogin(actualCriteria);
                var loginModelDetails = (LoginModel)(serialization.DeSerializeBinary(Convert.ToString(result)));

                var validateResult = false;
                var isValidPassword = false;
                if (loginModelDetails.common != null)
                    isValidPassword = SessionManagement.CipherTool.Verify(loginModel.Password, Convert.ToString(loginModelDetails.common.Password));

                if (isValidPassword)
                {
                    if (loginModelDetails.common.IsEnabled == false)
                    {
                        ModelState.AddModelError("", "User account is disabled, Please contact Administrator.");
                        return PartialView("_Login", loginModel);
                    }

                    //initialize userAuthModel
                    var userSession = Authenticate(loginModelDetails);

                    //
                    if (userSession != null)
                    {
                        var identity = new ClaimsIdentity(AuthenticationHelper.CreateClaim(userSession,
                                                            userSession.UserRole),
                                                            DefaultAuthenticationTypes.ApplicationCookie
                                                            );
                        AuthenticationManager.SignIn(new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddHours(1)
                        }, identity);
                    }

                    SessionController.UserSession.UserId = loginModelDetails.common.UserId;
                    SessionController.UserSession.UserName = loginModelDetails.common.UserName;
                    SessionController.UserSession.EmailAddress = loginModelDetails.common.EmailAddress;
                    SessionController.UserSession.RoleType = loginModelDetails.common.RoleType;
                    validateResult = true;

                    //Reteive the subscription for the user to check if this is valid or not
                    HashCriteria = new Hashtable();
                    actualCriteria = string.Empty;
                    List<UserProfileEditModel> objUserProfileDetails = new List<UserProfileEditModel>();
                    HashCriteria.Add("UserID", loginModelDetails.common.UserId);
                    actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                    var resultuser = userRegistration.GetUserSpecificDetails(actualCriteria);
                    objUserProfileDetails = (List<UserProfileEditModel>)(serialization.DeSerializeBinary(Convert.ToString(resultuser)));
                    var UserProfileDetails = objUserProfileDetails.FirstOrDefault();

                    //To get the customer credit card information for this user
                    if (UserProfileDetails.CustomerID != "" && UserProfileDetails.CustomerID != null)
                    {
                        string customerID = UserProfileDetails.CustomerID;
                        SessionController.UserSession.CustomerID = customerID;

                        var customerService = new StripeCustomerService();
                        StripeCustomer stripeCustomer = customerService.Get(customerID);

                        //Check if user has any subscription or not
                        if (stripeCustomer.Subscriptions.TotalCount > 0)
                        {
                            var subscriptionID = stripeCustomer.Subscriptions.Data[0].Id;

                            var subscriptionService = new StripeSubscriptionService();
                            StripeSubscription stripeSubscription = subscriptionService.Get(subscriptionID);

                            //Check if the user subscription is on or not: If on then Paid else Unpaid
                            if (stripeSubscription.Status == "active")
                                SessionController.UserSession.IsPaid = true;
                            else
                                SessionController.UserSession.IsPaid = false;
                        }
                        else
                        {
                            SessionController.UserSession.IsPaid = false;
                        }
                    }
                    else
                    {
                        SessionController.UserSession.IsPaid = false;
                    }
                }

                if (validateResult)
                {
                    var url = new
                    {
                        Url = Request.Url.AbsoluteUri,
                        type = "Url"
                    };
                    return Json(url, JsonRequestBehavior.AllowGet);
                }
            }
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return PartialView("_Login", loginModel);
        }
        #endregion

        #region LoginError
        public ActionResult LoginError()
        {
            ModelState.AddModelError("", "An error occurred while processing your request.");
            return PartialView("_Login");
        }
        #endregion

        #region log out
        public ActionResult LogOff()
        {
            bool isLogedin = false;
            if (SessionController.UserSession.UserId != null)
                isLogedin = true;
            SessionController.UserSession.UserId = null;
            SessionController.UserSession.UserName = null;
            SessionController.UserSession.EmailAddress = null;
            SessionController.UserSession.RoleType = null;
            SessionController.UserSession.IsSubscribed = null;
            SessionController.UserSession.CustomerID = null;
            SessionController.UserSession.IsPaid = false;

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);

            Session.Abandon();
            //log out from discourse
            if (isLogedin)
                LogoutDiscourse();
            return RedirectToAction("HomePage", "HomePage");
        }


        public void LogoutDiscourse()
        {
            string apiKey = ConfigurationManager.AppSettings["discourse_api_key"];
            string apiUsername = ConfigurationManager.AppSettings["discourse_api_username"];
            string getUrlExternalId = "http://www.unbot.com/users/by-external/" + userId + ".json" + "?api_key=" + apiKey + "&api_username=" + apiUsername;
            int discourseId = GetDiscourseId(getUrlExternalId);
            string url = "http://www.unbot.com/admin/users/" + discourseId + "/log_out?api_key=" + apiKey + "&api_username=" + apiUsername;
            string response = CalloutDiscourseApi(url);

        }

        public int GetDiscourseId(string apiUrl)
        {
            int disCourseId = 0;
            //HttpWebResponse response = null;
            StreamReader respStream = null;

            // Create a request object using the url passed in 
            var request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "GET";

            //THis line needs to be reviewed at stage/production, as it is used to bypass protocol security from local environment.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //Set the content type of the data being posted.
            request.ContentType = "multipart/form-data";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Pass the response into a stream reader
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string rawJson = reader.ReadToEnd();

                    var json = JObject.Parse(rawJson);

                    var userJson = json.GetValue("user");

                    disCourseId = userJson.First().First().Value<int>();
                }
            }
            return disCourseId;
        }

        public string CalloutDiscourseApi(string apiUrl)
        {
            //HttpWebResponse response = null;
            StreamReader respStream = null;

            // Create a request object using the url passed in 
            var request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "POST";

            //THis line needs to be reviewed at stage/production, as it is used to bypass protocol security from local environment.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //Set the content type of the data being posted.
            request.ContentType = "multipart/form-data";

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                // Create a streamreader object from the response 
                respStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                // Get the contents of the page as a string and return it 
                return respStream.ReadToEnd();
            }

        }


        public UserAuthenticationModel Authenticate(LoginModel loginModel)
        {
            return new UserAuthenticationModel
            {
                UserId = Guid.NewGuid(),
                DisplayName = loginModel.common.UserName,
                UserRole = loginModel.common.RoleType
            };
        }

        public IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

    }

    #endregion
}
