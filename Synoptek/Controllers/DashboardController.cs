using BusinessLogic.Models;
using BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;
using Synoptek.SessionManagement;
using System.Security.Claims;

namespace Synoptek.Controllers
{
    [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin + "," + Helpers.Constants.UserRoles.Broker + "," + Helpers.Constants.UserRoles.Investor + "," + Helpers.Constants.UserRoles.RegisterUser)]
    public class DashboardController : BaseController
    {
        #region Common variables 
        string userID = Convert.ToString(SessionController.UserSession.UserId);
        #endregion

        #region Get Broker Dashboard
        public ActionResult Broker(int? page, string Status = null)
        {
            var serialization = new Serialization();
            var brokerListings = new BrokerListings();
            var brokerListingsWrapper = new BrokerListingsWrapper();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            brokerListingsWrapper.Active = new List<BrokerListingsModel>();
            brokerListingsWrapper.Drafts = new List<BrokerListingsModel>();
            brokerListingsWrapper.Inactive = new List<BrokerListingsModel>();
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = brokerListings.GetPostingDetails(actualCriteria);
            var brokerListing = (List<BrokerListingsModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            foreach (var item in brokerListing)
            {
                switch (item.ListingStatus)
                {
                    case "Active":
                        brokerListingsWrapper.Active.Add(item);
                        break;
                    case "Draft":
                        brokerListingsWrapper.Drafts.Add(item);
                        break;
                    case "Inactive":
                        brokerListingsWrapper.Inactive.Add(item);
                        break;
                }
            }
            var pageSize = 12;
            var pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            brokerListingsWrapper.PagedListActive = brokerListingsWrapper.Active.ToPagedList(pageIndex, pageSize);
            brokerListingsWrapper.PagedListDrafts = brokerListingsWrapper.Drafts.ToPagedList(pageIndex, pageSize);
            brokerListingsWrapper.PagedListInactive = brokerListingsWrapper.Inactive.ToPagedList(pageIndex, pageSize);
            if (Status == "Active" || Status == null)
            {
                foreach (var item in brokerListingsWrapper.PagedListActive)
                {
                    item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
                }
            }
            if (Status == "Active")
            {
                return PartialView("_BrokerMyListings", brokerListingsWrapper.PagedListActive);
            }
            if (Status == "Draft")
            {
                foreach (var item in brokerListingsWrapper.PagedListDrafts)
                {
                    item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
                }
                return PartialView("_BrokerMyListings", brokerListingsWrapper.PagedListDrafts);
            }
            if (Status == "InActive" || Status == "Inactive")
            {
                foreach (var item in brokerListingsWrapper.PagedListInactive)
                {
                    item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
                }
                return PartialView("_BrokerMyListings", brokerListingsWrapper.PagedListInactive);
            }
            return View("BrokerDashboard", brokerListingsWrapper);
        }
        #endregion

        #region Investors Dashboard
        [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Investor + "," + Helpers.Constants.UserRoles.RegisterUser)]
        public ActionResult Investor(int? page, string listingStatus = null)
        {
            var investorDashboardBA = new Dashboard();
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var dashboardWrapper = new DashboardWrapper();
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = investorDashboardBA.GetInvestorDashboard(actualCriteria);
            dashboardWrapper.InvestorListing = (List<InvestorDashboardModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            var tempActiveListings = new List<InvestorDashboardModel>();
            var tempFavoriteListings = new List<InvestorDashboardModel>();
            foreach (var item in dashboardWrapper.InvestorListing)
            {
                if (item.IsFavorite && item.SentMessageCount > 0)
                {
                    item.Status = "Favorite";
                    tempFavoriteListings.Add(item);
                    tempActiveListings.Add(item);
                    item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
                }
                else if (item.IsFavorite && item.SentMessageCount <= 0)
                {
                    item.Status = "Favorite";
                    tempFavoriteListings.Add(item);
                    item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
                }
                else if (!item.IsFavorite && item.SentMessageCount > 0)
                {
                    item.Status = "Active";
                    tempActiveListings.Add(item);
                    item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
                }
            }
            var pageSize = 12;
            var pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            dashboardWrapper.PagedListInvestorActiveListing = tempActiveListings.ToPagedList(pageIndex, pageSize);
            dashboardWrapper.PagedListInvestorFavoriteListing = tempFavoriteListings.ToPagedList(pageIndex, pageSize);
            if (listingStatus != null)
            {
                if (listingStatus == "Active")
                    return PartialView("_InvestorMyListings", dashboardWrapper.PagedListInvestorActiveListing);
                if (listingStatus == "Favorite")
                    return PartialView("_InvestorMyListings", dashboardWrapper.PagedListInvestorFavoriteListing);
            }
            return View("Investor", dashboardWrapper.PagedListInvestorActiveListing);
        }
        #endregion 

        #region Admin Dashboard
        [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin)]
        public ActionResult Admin(int? page, string Tab = null, string sortingOrder = "RecentlyPosted", string searchText = "")
        {
            searchText = searchText.Replace("'", "''");
            Serialization serialization = new Serialization();
            DashboardWrapper dashboardWrapper = new DashboardWrapper();
            Hashtable HashCriteria = new Hashtable();
            Hashtable UsersHashCriteria = new Hashtable();
            Hashtable ResourcesHashCriteria = new Hashtable();
            Hashtable TestimonialsHashCriteria = new Hashtable();

            string actualCriteria;

            actualCriteria = serialization.SerializeBinary((object)HashCriteria);

            int pageSize = 8;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            if (Tab == "Users")
            {
                UserRegistration userRecords = new UserRegistration();
                UsersHashCriteria.Add("Name", searchText);
                UsersHashCriteria.Add("SortingOrder", sortingOrder);
                actualCriteria = serialization.SerializeBinary((object)UsersHashCriteria);
                var users = userRecords.GetUserSpecificDetails(actualCriteria);
                dashboardWrapper.Users = (List<UserProfileEditModel>)(serialization.DeSerializeBinary(Convert.ToString(users)));
                dashboardWrapper.PagedListUsers = dashboardWrapper.Users.ToPagedList(pageIndex, pageSize);

                return PartialView("_AdminUsersTab", dashboardWrapper.PagedListUsers);
            }
            else if (Tab == "Resources")
            {
                Learn learnBA = new Learn();
                ResourcesHashCriteria.Add("SortingOrder", sortingOrder);
                ResourcesHashCriteria.Add("SearchText", searchText);
                ResourcesHashCriteria.Add("IsAdminDashboard", "1");
                actualCriteria = serialization.SerializeBinary((object)ResourcesHashCriteria);
                var resourcesList = learnBA.GetLearnListingDetails(actualCriteria);
                dashboardWrapper.Resources = (List<LearnModel>)(serialization.DeSerializeBinary(Convert.ToString(resourcesList)));
                dashboardWrapper.PagedListResource = dashboardWrapper.Resources.ToPagedList(pageIndex, pageSize);
                return PartialView("_AdminResourcesTab", dashboardWrapper.PagedListResource);
            }
            else if (Tab == "Testimonials")
            {
                Testimonial testimonialBA = new Testimonial();
                TestimonialsHashCriteria.Add("SortingOrder", sortingOrder);
                TestimonialsHashCriteria.Add("SearchText", searchText);
                actualCriteria = serialization.SerializeBinary((object)TestimonialsHashCriteria);
                dashboardWrapper.Testimonials = testimonialBA.GetAllTestimonial(actualCriteria);
                dashboardWrapper.PagedListTestimonial = dashboardWrapper.Testimonials.ToPagedList(pageIndex, pageSize);
                return PartialView("_AdminTestimonialsTab", dashboardWrapper.PagedListTestimonial);
            }
            else
            {
                InvestorListings investorListings = new InvestorListings();
                InvestorListingWrapper investorListingWrapper = new InvestorListingWrapper();
                investorListingWrapper.SortingOrder = sortingOrder;
                investorListingWrapper.ListingTypeIds = string.Empty;
                HashCriteria.Add("ID", investorListingWrapper.ListingTypeIds);
                HashCriteria.Add("LocationName", investorListingWrapper.LocationName);
                HashCriteria.Add("SortingOrder", investorListingWrapper.SortingOrder);
                HashCriteria.Add("Name", searchText);
                HashCriteria.Add("LoanID", investorListingWrapper.LoanID);
                HashCriteria.Add("LienPositionID", investorListingWrapper.LienPositionID);
                HashCriteria.Add("LoanStatusID", investorListingWrapper.LoanStatusID);
                HashCriteria.Add("State", investorListingWrapper.State);
                HashCriteria.Add("LoanTypeID", investorListingWrapper.LoanTypeID);
                HashCriteria.Add("Address", investorListingWrapper.Address);
                HashCriteria.Add("PropertyTypeID", investorListingWrapper.PropertyTypeID);
                HashCriteria.Add("MinPrincipalBalance", 0);
                HashCriteria.Add("MaxPrincipalBalance", 0);
                HashCriteria.Add("City", investorListingWrapper.City);
                HashCriteria.Add("MinInterestRate", 0);
                HashCriteria.Add("MaxInterestRate", 0);
                HashCriteria.Add("MinAskingPrice", 0);
                HashCriteria.Add("MaxAskingPrice", 0);
                HashCriteria.Add("UserID", userID);
                actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                var listing = investorListings.GetDealDetails(actualCriteria);
                dashboardWrapper.InvestorAdminListing = (List<InvestorListingsModel>)(serialization.DeSerializeBinary(Convert.ToString(listing)));
                dashboardWrapper.PagedListAdminListings = dashboardWrapper.InvestorAdminListing.ToPagedList(pageIndex, pageSize);

                //Users default load
                actualCriteria = string.Empty;
                UserRegistration userRecords = new UserRegistration();
                UsersHashCriteria.Add("SortingOrder", sortingOrder);
                actualCriteria = serialization.SerializeBinary((object)UsersHashCriteria);
                dashboardWrapper.Users = new List<UserProfileEditModel>();
                var users = userRecords.GetUserSpecificDetails(actualCriteria);
                dashboardWrapper.Users = (List<UserProfileEditModel>)(serialization.DeSerializeBinary(Convert.ToString(users)));
                dashboardWrapper.PagedListUsers = dashboardWrapper.Users.ToPagedList(pageIndex, pageSize);

                //Resources default load
                actualCriteria = string.Empty;
                Learn learnBA = new Learn();
                //For Learn
                LearnModel learn = new LearnModel();

                ResourcesHashCriteria.Add("SortingOrder", sortingOrder);
                ResourcesHashCriteria.Add("IsAdminDashboard", "1");
                actualCriteria = serialization.SerializeBinary((object)ResourcesHashCriteria);
                var resourcesList = learnBA.GetLearnListingDetails(actualCriteria);
                dashboardWrapper.Resources = (List<LearnModel>)(serialization.DeSerializeBinary(Convert.ToString(resourcesList)));
                dashboardWrapper.PagedListResource = dashboardWrapper.Resources.ToPagedList(pageIndex, pageSize);

                Testimonial testimonialBA = new Testimonial();
                TestimonialsHashCriteria.Add("SortingOrder", sortingOrder);
                TestimonialsHashCriteria.Add("SearchText", searchText);
                actualCriteria = serialization.SerializeBinary((object)TestimonialsHashCriteria);
                dashboardWrapper.Testimonials = testimonialBA.GetAllTestimonial(actualCriteria);
                dashboardWrapper.PagedListTestimonial = dashboardWrapper.Testimonials.ToPagedList(pageIndex, pageSize);

                if (Tab != null)
                    return PartialView("_AdminListingsTab", dashboardWrapper.PagedListAdminListings);
                return View("AdminDashboard", dashboardWrapper);
            }
        }
        #endregion

        #region Delete listings
        [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Broker + "," + Helpers.Constants.UserRoles.Admin)]
        public ActionResult Delete(string listingID)
        {
            var brokerListings = new BrokerListings();
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var listing_ID = Convert.ToInt64(CipherTool.DecryptString(listingID));
            HashCriteria.Add("ID", listing_ID);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = brokerListings.DeleteCurrentListing(actualCriteria);
            var articleID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Deal Messages
        public ActionResult GetDealMessages(string listingID)
        {
            var i = 0;
            var brokerListings = new BrokerListings();
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var listing_ID = Convert.ToInt64(CipherTool.DecryptString(listingID));
            HashCriteria.Add("ID", listing_ID);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = brokerListings.GetCurrentListingMessages(actualCriteria);
            var lstListingComments = (List<ListingComments>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            foreach (var item in lstListingComments)
            {
                item.Duration = SharedFunctions.GetDuration(item.MessageTimeStamp);
                item.ImagePath = CheckFileExists(item.ImagePath, "ProfileImagePath", Convert.ToString(item.FromID), true);
                if (lstListingComments[i].RepliedComments != null)
                {
                    foreach (var repliedItem in lstListingComments[i].RepliedComments)
                    {
                        repliedItem.Duration = SharedFunctions.GetDuration(repliedItem.MessageTimeStamp);
                        repliedItem.ImagePath = CheckFileExists(repliedItem.ImagePath, "ProfileImagePath", Convert.ToString(repliedItem.FromID), true);
                    }
                }
                i++;
            }
            return PartialView("_DealMessageBox", lstListingComments);
        }
        #endregion 
    }
}
