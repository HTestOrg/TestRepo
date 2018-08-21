using BusinessObjects;
using Synoptek.Filters;
using Synoptek.Helpers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Routing;

namespace Synoptek.Controllers
{
    [ExceptionHandling]
    public class ParentController : Controller
    {
        protected internal UserAuthenticationModel UserAuthModel { get; private set; }
        public string userRole = Convert.ToString(SessionController.UserSession.RoleType);

        public new RedirectToRouteResult RedirectToAction(string action, string controller)
        {
            return base.RedirectToAction(action, controller);
        }

        #region Check news image and get path of image to display
        protected string CheckFileExists(string filepath, string ModuleName, string listingID, bool IsUserProfile = false)
        {
            var ImageName = filepath;
            var _ImagePath = System.Configuration.ConfigurationManager.AppSettings[ModuleName];
            var serverPath = Server.MapPath(_ImagePath + "/" + listingID + "/" + ImageName);
            if (System.IO.File.Exists(serverPath))
            {
                var imgName = ImageName.Substring(ImageName.LastIndexOf('\\') + 1);
                return Request.Url.GetLeftPart(UriPartial.Authority) + _ImagePath + "/" + listingID + "/" + imgName;
            }
            else
            {
                var imgName = "";
                if (IsUserProfile)
                    imgName = "/Images/profile_men.jpg";
                else
                    imgName = "/Images/no_image_present.png";
                return Request.Url.GetLeftPart(UriPartial.Authority) + "/" + imgName;
            }
        }

        protected string CheckFileExistsGender(string filepath, string gender, string ModuleName, string listingID, string IsUserProfile = null)
         {
            var ImageName = filepath;
            var _ImagePath = System.Configuration.ConfigurationManager.AppSettings[ModuleName];
            var serverPath = Server.MapPath(_ImagePath + "/" + listingID + "/" + ImageName);
            if (System.IO.File.Exists(serverPath))
            {
                var imgName = ImageName.Substring(ImageName.LastIndexOf('\\') + 1);
                return Request.Url.GetLeftPart(UriPartial.Authority) + _ImagePath + "/" + listingID + "/" + imgName;
            }
            else
            {
                var imgName = "";
                if (IsUserProfile == "UserProfile")
                {


                    if (gender == "M" || gender == "")
                    {
                        imgName = "/Images/profile_men.jpg";
                    }
                    else if(gender=="F")
                    {
                        imgName = "/Images/profile_women.jpg";
                    }
                }
            
                else
                    imgName = "/Images/no_image_present.png";
                return Request.Url.GetLeftPart(UriPartial.Authority) + "/" + imgName;
            }
        }

       
        #endregion

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var user = User as ClaimsPrincipal;
            if (user != null)
            {
                var claims = user.Claims.ToList();
                var sessionClaim = claims.FirstOrDefault(o => o.Type == Constants.UserSession);
                if (sessionClaim != null)
                {
                    UserAuthModel = sessionClaim.Value.ToObject<UserAuthenticationModel>();
                }
            }
        }
    }

    [ExceptionHandling]
    [LoggingFilter]
    public class BaseController : ParentController
    {
        public new RedirectToRouteResult RedirectToAction(string action, string controller)
        {
            return base.RedirectToAction(action, controller);
        } 
    }
}
