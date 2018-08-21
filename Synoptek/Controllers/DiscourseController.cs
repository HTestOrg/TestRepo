using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Models;
using BusinessObjects;


namespace Synoptek.Controllers
{
    public class DiscourseController : BaseController
    {
        #region Common variables
        string userID = Convert.ToString(SessionController.UserSession.UserId);
        #endregion

        #region Index
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Discourse Login
        public ActionResult DiscourseLogin()
        {
            var userRegistration = new UserRegistration();
            var serialization = new Serialization();
            if (string.IsNullOrEmpty(Request.QueryString["sso"]) || string.IsNullOrEmpty(Request.QueryString["sig"]))
                return RedirectToAction("ShowSignup", "Discourse");

            //must match sso_secret in discourse settings
            var ssoSecret = ConfigurationManager.AppSettings["SSO_SECRET"].ToString();
            var sso = Request.QueryString["sso"];
            var sig = Request.QueryString["sig"];
            var checksum = getHash(sso, ssoSecret);
            if (checksum != sig)
                return RedirectToAction("ShowLogin", "Discourse");

            byte[] ssoBytes = Convert.FromBase64String(sso);
            var decodedSso = Encoding.UTF8.GetString(ssoBytes);
            NameValueCollection nvc = HttpUtility.ParseQueryString(decodedSso);
            var nonce = nvc["nonce"];
            //Code to get user information.
            //Ensure user is logged in by adding the [Authorize]   
            //Attribute to this controller method and validate the
            //user has permission to access the forum      

            var HashCriteria = new Hashtable();
            string actualCriteria;
            var objUserProfileDetails = new List<UserProfileEditModel>();
            HashCriteria.Add("UserID", userID);

            actualCriteria = serialization.SerializeBinary((object)HashCriteria);

            string userGroups = "registerusers";
            userGroups = (Synoptek.SessionController.UserSession.RoleType + "s").ToLower();
            if (Synoptek.SessionController.UserSession.RoleType == Synoptek.Helpers.Constants.UserRoles.Admin)
                userGroups = "administrators";

            var result = userRegistration.GetUserSpecificDetails(actualCriteria);
            objUserProfileDetails = (List<UserProfileEditModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            var UserProfileDetails = objUserProfileDetails.FirstOrDefault();
            var returnPayload = "nonce=" + Server.UrlEncode(nonce) +
                                     "&email=" + Server.UrlEncode(UserProfileDetails.Email) +
                                     "&external_id=" + Server.UrlEncode(UserProfileDetails.ID.ToString()) +
                                     "&username=" + Server.UrlEncode(UserProfileDetails.Email) +
                                     "&name=" + Server.UrlEncode(UserProfileDetails.Name) +
                                     "&add_groups=" + Server.UrlEncode(userGroups);

            var encodedPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(returnPayload));
            var returnSig = getHash(encodedPayload, ssoSecret);

            var redirectUrl = ConfigurationManager.AppSettings["DiscourseUrl"] + "/session/sso_login?sso=" + encodedPayload + "&sig=" + returnSig;

            return Redirect(redirectUrl);
        }
        #endregion

        #region getHash
        public string getHash(string payload, string ssoSecret)
        {
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(ssoSecret);
            System.Security.Cryptography.HMACSHA256 hasher = new System.Security.Cryptography.HMACSHA256(keyBytes);
            byte[] bytes = encoding.GetBytes(payload);
            byte[] hash = hasher.ComputeHash(bytes);
            string ret = string.Empty;
            foreach (byte x in hash)
                ret += String.Format("{0:x2}", x);
            return ret;
        }
        #endregion

        #region ShowLogin
        public ActionResult ShowLogin()
        {
            return View("DiscourseLogin");
        }
        #endregion

        #region Show sign up
        public ActionResult ShowSignup()
        {
            return View("DiscourseSignup");
        }
        #endregion
    }
}