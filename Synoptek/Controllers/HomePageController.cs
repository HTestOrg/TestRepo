using System;
using System.Web.Mvc;
using BusinessObjects;
using BusinessLogic.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Collections;
using Synoptek.SessionManagement;
using System.Linq;
using NLog;


namespace Synoptek.Controllers
{
    public class HomePageController : ParentController
    {
        #region Common variables 
        string userID = Convert.ToString(SessionController.UserSession.UserId);
        Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Get News Details
        public ActionResult HomePage()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated && SessionController.UserSession.UserId == null)
            {
                return RedirectToAction("LogOff", "Login");
            }
            var testimonialBA = new Testimonial();
            var homePageBA = new HomePage();
            var homeModel = new HomePageModel();
            var testimonialModel = new TestimonialModel();
            var lstResources = new List<HomePageLatestResources>();
            Hashtable TestimonialsHashCriteria = new Hashtable();
            Serialization serialization = new Serialization();
            string actualCriteria;
            homeModel.resourceList = homePageBA.GetLatestNewsandArticlesForHomePage();
            homeModel.brokerList = homePageBA.GetListingDetailsForHomePage();
            //Get latest 3 resources with images, show teaser content and teaser content is less than 3 then show basic content
            foreach (var resource in homeModel.resourceList)
            {
                if (resource.ContentType == 0)
                    lstResources.Add(resource);
            }
            if (lstResources.Count < 3)
            {
                foreach (var resource in homeModel.resourceList)
                {
                    if (resource.ContentType == 1)
                        lstResources.Add(resource);
                }
            }
            homeModel.resourceList = lstResources;
            foreach (var resource in homeModel.resourceList)
            {
                if (resource.ContentFor == (Convert.ToInt32(ContentFor.All)))
                {
                    resource.ContentName = Convert.ToString(ContentFor.All);
                }
                else if (resource.ContentFor == (Convert.ToInt32(ContentFor.Investor)))
                {
                    resource.ContentName = Convert.ToString(ContentFor.Investor);

                }
                else if (resource.ContentFor == (Convert.ToInt32(ContentFor.Broker)))
                {
                    resource.ContentName = Convert.ToString(ContentFor.Broker);
                }
                resource.LearnCategory = resource.LearnCategory;
            }

            if (homeModel.brokerList.Count > 3)
                homeModel.brokerList = homePageBA.GetListingDetailsForHomePage().GetRange(0, 4);

            foreach (var item in homeModel.brokerList)
            {
                item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
            }
            foreach (var item in homeModel.resourceList)
            {
                item.ImagePath = CheckFileExists(item.ImagePath, "LearnImagePath", Convert.ToString(item.ID));
            }

            TestimonialsHashCriteria.Add("SortingOrder", "RecentlyPosted");
            TestimonialsHashCriteria.Add("SearchText", null);
            actualCriteria = serialization.SerializeBinary((object)TestimonialsHashCriteria);

            homeModel.TestimonialList = testimonialBA.GetAllTestimonial(actualCriteria);

            if (homeModel.TestimonialList.Count > 5)
            {
                homeModel.TestimonialList = testimonialBA.GetAllTestimonial().OrderByDescending(x => x.CreatedDate).ToList();
                homeModel.TestimonialList = homeModel.TestimonialList.GetRange(0, 5);
            }

            foreach (var item in homeModel.TestimonialList)
            {
                item.ImagePath = CheckFileExists(item.ImagePath, "TestimonialImagePath", Convert.ToString(item.ID));
            }
            return View(homeModel);
        }
        #endregion

        #region Select Your Role on click of Lender/Exchange menu
        [Display(Name = "Lender/Investor Exchange")]
        public ActionResult SelectYourRole()
        {
            var testimonialBA = new Testimonial();
            var sharedFunction = new SharedFunctions();
            Hashtable TestimonialsHashCriteria = new Hashtable();
            Serialization serializationTestimonial = new Serialization();
            string actualCriteria;

            if (SessionController.UserSession.RoleType != null)
            {
                //Navigate admin to his dashboard
                if (SessionController.UserSession.RoleType == "Admin")
                {
                    return RedirectToAction("InvestorsListingDetails", "InvestorListings");
                }
                //To check if user is paid or not then show the contents
                var userID = Convert.ToInt64(SessionController.UserSession.UserId);
                if (userID > 0 && Convert.ToString(userID) != null)
                {
                    List<UserProfileEditModel> userprofileModel = sharedFunction.GetUserRole(userID);
                    UserProfileEditModel userprofile = userprofileModel.FirstOrDefault();
                    var IsPaid = Convert.ToBoolean(userprofile.IsPaid);
                    var IsActiveSubscription = Convert.ToBoolean(SessionController.UserSession.IsPaid);
                    if (IsActiveSubscription)
                    {
                        if (IsPaid)
                        {
                            if (SessionController.UserSession.RoleType == "Investor")
                            {
                                return RedirectToAction("InvestorsListingDetails", "InvestorListings");
                            }
                            if (SessionController.UserSession.RoleType == "Broker")
                            {
                                return RedirectToAction("Broker", "Dashboard");
                            }
                        }
                    }
                }
            }
            var homeModel = new HomePageModel();
            TestimonialsHashCriteria.Add("SortingOrder", "RecentlyPosted");
            TestimonialsHashCriteria.Add("SearchText", null);
            actualCriteria = serializationTestimonial.SerializeBinary((object)TestimonialsHashCriteria);

            homeModel.TestimonialList = testimonialBA.GetAllTestimonial(actualCriteria);

            if (homeModel.TestimonialList.Count > 5)
            {
                homeModel.TestimonialList = testimonialBA.GetAllTestimonial().OrderByDescending(x => x.CreatedDate).ToList();
                homeModel.TestimonialList = homeModel.TestimonialList.GetRange(0, 5);
            }

            foreach (var item in homeModel.TestimonialList)
            {
                item.ImagePath = CheckFileExists(item.ImagePath, "TestimonialImagePath", Convert.ToString(item.ID));
            }

            var serialization = new Serialization();
            var investorListingsBA = new InvestorListings();
            var result = investorListingsBA.GetAllFeaturedList();
            homeModel.FeaturedList = (List<InvestorListingsModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            if (homeModel.FeaturedList.Count < 5)
                homeModel.FeaturedList = homeModel.FeaturedList.GetRange(0, homeModel.FeaturedList.Count);
            else
                homeModel.FeaturedList = homeModel.FeaturedList.GetRange(0, 4);
            foreach (var item in homeModel.FeaturedList)
            {
                item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
            }
            return View("SelectYourRole", homeModel);
        }
        #endregion

        #region Update user role as Investor/Broker
        public ActionResult UpdateUserRole(long roleID)
        {
            var userProfileBA = new UserRegistration();
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            string actualCriteria;
            HashCriteria.Add("RoleID", roleID);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = userProfileBA.UpdateUserRole(actualCriteria);
            result = Convert.ToString(serialization.DeSerializeBinary(Convert.ToString(result)));
            if (roleID == 2)
            {
                return RedirectToAction("Investor", "Dashboard");
            }
            if (roleID == 3)
            {
                return RedirectToAction("Broker", "Dashboard");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region
        public ActionResult GetSubscriptionPupup(long roleID)
        {
            ViewBag.RoleID = roleID;
            var result = "test";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check users access and assign the screens accordingly, Publis:Sign Up, Registered : Paywall, Paid: Content show(Dashboard)
        public ActionResult GetUserRole(long roleID, long? userID)
        {
            var result = string.Empty;
            if (userID == null)
            {
                result = "Public";
            }
            ViewBag.RoleID = roleID;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Schedule Consultation for Public User
        public ActionResult ScheduleConsultationForPublicUser(string Email, string Name, string Phone, string Message)
        {
            var investorListingsBA = new InvestorListings();
            var investorlisting = new InvestorListingsController();
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            string actualCriteria;
            HashCriteria.Add("ID", null);
            HashCriteria.Add("Message", Message);
            HashCriteria.Add("Subject", "Schedule Consultation Request ");
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("Name", Name);
            HashCriteria.Add("PhoneNumber", Phone);
            HashCriteria.Add("EmailAddress", Email);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var resultMessage = investorListingsBA.SendMessageToBroker(actualCriteria);
            var CommentID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(resultMessage)));
            var result = investorlisting.SendContactRequestEmail(CommentID, false);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Error methods
        public ActionResult Error(string errorCode = "", string errMessage = "", string actionPath = "")
        {
            if (!string.IsNullOrWhiteSpace(errMessage))
            {
                var ajaxException = new CustomAppException();
                ajaxException.ErrorID = errorCode;
                ajaxException.ErrorMessage = errMessage;
                ajaxException.ErrorSource = actionPath;
                logger.Error("Error: " + ajaxException, "Error occured in Controller: HomePage, Action: Error");
            }
            return View("Error");
        }
        #endregion

        public ActionResult Unauthorize()
        {
            return View("_Unauthorize");
        }
    }
}
