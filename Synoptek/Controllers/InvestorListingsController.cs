using System;
using System.Web.Mvc;
using BusinessObjects;
using BusinessLogic.Models;
using PagedList;
using Synoptek.SessionManagement;
using Synoptek.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Synoptek.Controllers
{
    [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Investor +","+ Helpers.Constants.UserRoles.Admin + "," + Helpers.Constants.UserRoles.Broker)]
    public class InvestorListingsController : ParentController
    {
        #region Common variables 
        string userID = Convert.ToString(SessionController.UserSession.UserId);
        #endregion

        #region Investors Listing Details
        public ActionResult InvestorsListingDetails(int? page, InvestorListingWrapper model = null)
        {
            var listingsBA = new Listings();
            var serialization = new Serialization();
            var investorListingsBA = new InvestorListings();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var investorListingWrapper = new InvestorListingWrapper();
            string minPrincipalBalance = string.Empty, maxPrincipalBalance = string.Empty, minInterestRate = string.Empty, maxInterestRate = string.Empty;
            double minAskingPrice = 0.0, maxAskingPrice = 0.0;
            string[] principle_tokens, asking_tokens, interest_tokens;
            if (page != null)
            {
                model.LoanID = Convert.ToInt32(Session["LoanID"]);
                model.LienPositionID = Convert.ToInt32(Session["LienPositionID"]);
                model.LoanStatusID = Convert.ToInt32(Session["LoanStatusID"]);
                model.State = Convert.ToString(Session["State"]);
                model.LoanTypeID = Convert.ToInt32(Session["LoanTypeID"]);
                model.Address = Convert.ToString(Session["Address"]);
                model.PropertyTypeID = Convert.ToInt32(Session["PropertyTypeID"]);
                model.PricipalBalance = Convert.ToString(Session["PricipalBalance"]);
                model.City = Convert.ToString(Session["City"]);
                model.AskingPrice = Convert.ToString(Session["AskingPrice"]);
                model.InterestRate = Convert.ToString(Session["InterestRate"]);
            }
            else
            {
                RemoveTempDataForInvestor();
            }

            model.SortingOrder = "RecentlyPosted";
            var pageSize = 3;
            var pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //Split the data for Principal Balanace, Asking value and Interest rate
            string principleBalance = Convert.ToString(model.PricipalBalance);
            if (principleBalance != "0" && principleBalance != null)
            {
                principle_tokens = principleBalance.Split('-');
                minPrincipalBalance = principle_tokens[0];
                maxPrincipalBalance = principle_tokens[1];
            }
            else
            {
                minPrincipalBalance = "0";
                maxPrincipalBalance = "0";
            }

            string askingPrice = Convert.ToString(model.AskingPrice);
            if (askingPrice != "0" && askingPrice != null)
            {
                asking_tokens = askingPrice.Split('-');
                minAskingPrice = Convert.ToDouble(asking_tokens[0]);
                maxAskingPrice = Convert.ToDouble(asking_tokens[1]);
            }
            else
            {
                minAskingPrice = 0.0;
                maxAskingPrice = 0.0;
            }

            string interestRate = Convert.ToString(model.InterestRate);
            if (interestRate != "0" && interestRate != null)
            {
                interest_tokens = interestRate.Split('-');
                minInterestRate = interest_tokens[0];
                maxInterestRate = interest_tokens[1];
            }
            else
            {
                minInterestRate = "0";
                maxInterestRate = "0";
            }

            var res = investorListingsBA.GetAllFeaturedList();
            investorListingWrapper.FeaturedListings = (List<InvestorListingsModel>)(serialization.DeSerializeBinary(Convert.ToString(res)));
            investorListingWrapper.PagedListFeaturedListings = investorListingWrapper.FeaturedListings.ToPagedList(1, 4);

            HashCriteria.Add("LoanID", model.LoanID);
            HashCriteria.Add("LienPositionID", model.LienPositionID);
            HashCriteria.Add("LoanStatusID", model.ActualLoanStatusID);
            HashCriteria.Add("State", model.State);
            HashCriteria.Add("LoanTypeID", model.ListingTypeID);
            HashCriteria.Add("Address", model.Address);
            HashCriteria.Add("PropertyTypeID", model.PropertyTypeID);
            HashCriteria.Add("MinPrincipalBalance", minPrincipalBalance);
            HashCriteria.Add("MaxPrincipalBalance", maxPrincipalBalance);
            HashCriteria.Add("City", model.City);
            HashCriteria.Add("MinInterestRate", minInterestRate);
            HashCriteria.Add("MaxInterestRate", maxInterestRate);
            HashCriteria.Add("MinAskingPrice", minAskingPrice);
            HashCriteria.Add("MaxAskingPrice", maxAskingPrice);
            HashCriteria.Add("SortingOrder", model.SortingOrder);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = investorListingsBA.GetDealDetails(actualCriteria);
            investorListingWrapper.InvestorListings = (List<InvestorListingsModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            var tempInvestorListings = new List<InvestorListingsModel>();
            foreach (var item in investorListingWrapper.InvestorListings)
            {
                tempInvestorListings.Add(item);
                item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
            }
            investorListingWrapper.PagedListInvestorListings = tempInvestorListings.ToPagedList(pageIndex, pageSize);
            foreach (var item in investorListingWrapper.PagedListFeaturedListings)
            {
                item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
            }
            investorListingWrapper.ListingTypeList = investorListingsBA.GetListingTypes();
            var resultListingLoanInfo = listingsBA.GetListingDefaultValuesForLoanInformation();
            var listingModel = (LoanInformation)(serialization.DeSerializeBinary(Convert.ToString(resultListingLoanInfo)));
            investorListingWrapper.LienPosition = listingModel.LienPositionType;
            investorListingWrapper.ListingType = listingsBA.GetListingLoanTypes();
            var resultListing = listingsBA.GetListingDefaultValuesForPropertyInformation();
            var listingModelnew = (PropertyInformation)(serialization.DeSerializeBinary(Convert.ToString(resultListing)));
            investorListingWrapper.PropertyType = listingModelnew.PropertyType;
            return View("InvestorsListingDetails", investorListingWrapper);
        }
        #endregion

        #region Learn Listing Details
        public List<InvestorListingsModel> GetFeaturedList(InvestorListingWrapper investorListingWrapper, string listingId = "")
        {
            var counter = 0;
            var featuredList = new List<InvestorListingsModel>();
            foreach (var item in investorListingWrapper.InvestorListings)
            {
                if (item.IsSponsored && listingId != item.ID.ToString())
                {
                    if (counter < 4)
                    {
                        featuredList.Add(item);
                    }
                    counter++;
                }
            }
            return featuredList;
        }
        #endregion

        #region Investors Listing Details Partial
        public ActionResult InvestorsListingDetailsPartial(int? page, string SortingBy = null, InvestorListingWrapper model = null)
        {
            var serialization = new Serialization();
            var investorListingsBA = new InvestorListings();
            var HashCriteria = new Hashtable();
            string actualCriteria = string.Empty;
            string minPrincipalBalance = String.Empty, maxPrincipalBalance = String.Empty, minInterestRate = String.Empty, maxInterestRate = String.Empty;
            double minAskingPrice = 0.0, maxAskingPrice = 0.0;
            var investorListingWrapper = new InvestorListingWrapper();
            string[] principle_tokens, asking_tokens, interest_tokens;
            if (SortingBy != null)
            {
                model.SortingOrder = SortingBy;
            }
            //Split the data for Principal Balanace, Asking value and Interest rate
            var principleBalance = Convert.ToString(model.PricipalBalance);
            if (principleBalance != "0" && principleBalance != null)
            {
                principle_tokens = principleBalance.Split('-');
                minPrincipalBalance = principle_tokens[0];
                maxPrincipalBalance = principle_tokens[1];
            }
            else
            {
                minPrincipalBalance = "0";
                maxPrincipalBalance = "0";
            }

            var askingPrice = Convert.ToString(model.AskingPrice);
            if (askingPrice != "0" && askingPrice != null)
            {
                asking_tokens = askingPrice.Split('-');
                minAskingPrice = Convert.ToDouble(asking_tokens[0]);
                maxAskingPrice = Convert.ToDouble(asking_tokens[1]);
            }
            else
            {
                minAskingPrice = 0.0;
                maxAskingPrice = 0.0;
            }

            var interestRate = Convert.ToString(model.InterestRate);
            if (interestRate != "0" && interestRate != null)
            {
                interest_tokens = interestRate.Split('-');
                minInterestRate = interest_tokens[0];
                maxInterestRate = interest_tokens[1];
            }
            else
            {
                minInterestRate = "0";
                maxInterestRate = "0";
            }

            HashCriteria.Add("LoanID", model.LoanID);
            HashCriteria.Add("LienPositionID", model.LienPositionID);
            HashCriteria.Add("LoanStatusID", model.ActualLoanStatusID);
            HashCriteria.Add("State", model.State);
            HashCriteria.Add("LoanTypeID", model.ListingTypeID);
            HashCriteria.Add("Address", model.Address);
            HashCriteria.Add("PropertyTypeID", model.PropertyTypeID);
            HashCriteria.Add("MinPrincipalBalance", minPrincipalBalance);
            HashCriteria.Add("MaxPrincipalBalance", maxPrincipalBalance);
            HashCriteria.Add("City", model.City);
            HashCriteria.Add("MinInterestRate", minInterestRate);
            HashCriteria.Add("MaxInterestRate", maxInterestRate);
            HashCriteria.Add("MinAskingPrice", minAskingPrice);
            HashCriteria.Add("MaxAskingPrice", maxAskingPrice);
            HashCriteria.Add("SortingOrder", model.SortingOrder);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = investorListingsBA.GetDealDetails(actualCriteria);
            investorListingWrapper.InvestorListings = (List<InvestorListingsModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            var tempInvestorListings = new List<InvestorListingsModel>();
            foreach (var item in investorListingWrapper.InvestorListings)
            {
                tempInvestorListings.Add(item);
                item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
            }
            int pageSize = 3;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            investorListingWrapper.PagedListInvestorListings = tempInvestorListings.ToPagedList(pageIndex, pageSize);
            return PartialView("_InvestorListings", investorListingWrapper.PagedListInvestorListings);
        }
        #endregion

        #region Set TempData For Investor
        public ActionResult SetTempDataForInvestor(InvestorListingWrapper investorListingWrapper)
        {
            Session["LoanID"] = investorListingWrapper.LoanID;
            Session["LienPositionID"] = investorListingWrapper.LienPositionID;
            Session["LoanStatusID"] = investorListingWrapper.ActualLoanStatusID;
            Session["State"] = investorListingWrapper.State;
            Session["LoanTypeID"] = investorListingWrapper.ListingTypeID;
            Session["Address"] = investorListingWrapper.Address;
            Session["PropertyTypeID"] = investorListingWrapper.PropertyTypeID;
            Session["PricipalBalance"] = investorListingWrapper.PricipalBalance;
            Session["City"] = investorListingWrapper.City;
            Session["InterestRate"] = investorListingWrapper.InterestRate;
            Session["AskingPrice"] = investorListingWrapper.AskingPrice;
            return Content("");
        }
        #endregion

        #region Remove TempData For Investor
        public void RemoveTempDataForInvestor()
        {
            Session["LoanID"] = "";
            Session["LienPositionID"] = "";
            Session["LoanStatusID"] = "";
            Session["State"] = "";
            Session["LoanTypeID"] = "";
            Session["Address"] = "";
            Session["PropertyTypeID"] = "";
            Session["PricipalBalance"] = "";
            Session["City"] = "";
            Session["InterestRate"] = "";
            Session["AskingPrice"] = "";
        }
        #endregion

        #region Favorite
        public ActionResult Favorite(string listingID, bool IsFavorite)
        {
            var serialization = new Serialization();
            var investorListingsBA = new InvestorListings();
            var investorListingsModel = new InvestorListingsModel();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var listing_ID = Convert.ToInt64(CipherTool.DecryptString(listingID));
            HashCriteria.Add("ID", listing_ID);
            HashCriteria.Add("IsFavorite", !IsFavorite);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = investorListingsBA.MarkAsFavorite(actualCriteria);
            result = Convert.ToString(serialization.DeSerializeBinary(Convert.ToString(result)));
            if (result == "1" && IsFavorite == false)
                result = "1";
            else
                result = "0";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Featured Listing
        public List<InvestorListingsModel> GetFeaturedListingBottomSingleListing(string listingId = "")
        {
            var serialization = new Serialization();
            var investorListingsBA = new InvestorListings();
            var investorListingWrapper = new InvestorListingWrapper();
            var HashCriteria = new Hashtable();
            var res = investorListingsBA.GetAllFeaturedList();
            investorListingWrapper.InvestorListings = (List<InvestorListingsModel>)(serialization.DeSerializeBinary(Convert.ToString(res)));
            if (investorListingWrapper.InvestorListings.Count > 4)
                investorListingWrapper.FeaturedListings = investorListingWrapper.InvestorListings.GetRange(0, 4);
            else
                investorListingWrapper.FeaturedListings = investorListingWrapper.InvestorListings;
            foreach (var item in investorListingWrapper.FeaturedListings)
            {
                item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
            }
            return investorListingWrapper.FeaturedListings;
        }
        #endregion

        #region Get Single Listing Details
        public ActionResult GetSingleListingDetails(string listingId)
        {
            var singleListingWrapper = new SingleListingWrapper();
            var investorListingsModel = new InvestorListingsModel();
            investorListingsModel = SingleListingOperations(listingId);
            singleListingWrapper.SingleInvestorListings = investorListingsModel;
            singleListingWrapper.FeaturedOpportunityList = GetFeaturedListingBottomSingleListing(listingId);
            return View("InvestorViewSingleListing", singleListingWrapper);
        }
        #endregion

        #region Get Single Listing Details Partial
        public ActionResult GetSingleListingDetailsPartial(string listingId)
        {
            var investorListingsModel = new InvestorListingsModel();
            investorListingsModel = SingleListingOperations(listingId);
            return PartialView("_SingleListingDetails", investorListingsModel);
        }
        #endregion

        #region Single Listing Operations
        public InvestorListingsModel SingleListingOperations(string listingId)
        {
            var serialization = new Serialization();
            var investorListingsBA = new InvestorListings();
            var investorListingsModel = new InvestorListingsModel();
            investorListingsModel.noteGeneralInfo = new NoteGeneralInformation();
            investorListingsModel.noteTermsTab = new NoteTermsTab();
            investorListingsModel.noteDatesTab = new NoteDatesTab();
            investorListingsModel.propertyTab = new PropertyTab();
            investorListingsModel.foreClosureTab = new ForeClosureTab();
            var HashCriteria = new Hashtable();
            var HashCriteriaLoanImages = new Hashtable();
            var listingsBA = new Listings();
            string actualCriteria, actualCriteriaLoanImages;
            var listing_ID = CipherTool.DecryptString(listingId);
            HashCriteria.Add("ID", listing_ID);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = investorListingsBA.GetSingleDealDetails(actualCriteria);
            investorListingsModel = (InvestorListingsModel)(serialization.DeSerializeBinary(Convert.ToString(result)));
            HashCriteriaLoanImages.Add("UserID", userID);
            HashCriteriaLoanImages.Add("ID", listing_ID);
            actualCriteriaLoanImages = serialization.SerializeBinary((object)HashCriteriaLoanImages);
            var resultImages = listingsBA.GetLoanImages(actualCriteriaLoanImages);
            investorListingsModel.ImageList = (List<ListingImage>)(serialization.DeSerializeBinary(Convert.ToString(resultImages)));
            foreach (var item in investorListingsModel.ImageList)
            {
                item.ImagePath = CheckFileExists(item.FileName, "ListingImagePath", Convert.ToString(listing_ID));
            }
            if (investorListingsModel.ImagePath != "")
            {
                investorListingsModel.ImagePath = CheckFileExists(investorListingsModel.ImagePath, "ListingImagePath", Convert.ToString(listing_ID));
            }
            else if (investorListingsModel.ImagePath == "" && investorListingsModel.ImageList.Count > 0)
            {
                investorListingsModel.ImagePath = investorListingsModel.ImageList[0].ImagePath;
            }
            else
            {
                investorListingsModel.ImagePath = CheckFileExists(investorListingsModel.ImagePath, "ListingImagePath", Convert.ToString(listing_ID));
            }
            RemoveTempDataForInvestor();
            return investorListingsModel;
        }
        #endregion

        #region Brokerage Firm
        public ActionResult BrokerDetails(int? page, string BrokerName, string BrokerId, string Featured = "1")
        {

            var serialization = new Serialization();
            var brokerDetailsWrapper = new BrokerDetailsWrapper();
            var brokerListingsBA = new BrokerListings();
            brokerDetailsWrapper.BrokerName = BrokerName;
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var pageSize = 8;
            var pageIndex = 1;
            var Broker_ID = Convert.ToInt64(CipherTool.DecryptString(BrokerId));
            var Broker_Name = Convert.ToString(CipherTool.DecryptString(BrokerName));
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            brokerDetailsWrapper.BrokerID = Broker_ID;
            brokerDetailsWrapper.BrokerName = Broker_Name;
            HashCriteria.Add("UserID", Broker_ID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = brokerListingsBA.GetPostingDetails(actualCriteria);
            brokerDetailsWrapper.ListingDetails = new List<BrokerListingsModel>();
            brokerDetailsWrapper.ListingDetails = (List<BrokerListingsModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            var tempInvestorListings = new List<BrokerListingsModel>();
            var featuredListings = new List<BrokerListingsModel>();
            foreach (var item in brokerDetailsWrapper.ListingDetails)
            {
                if (item.IsSponsored)
                {
                    if (featuredListings.Count < 2)
                        featuredListings.Add(item);
                }
                tempInvestorListings.Add(item);
            }
            if (Featured == "1")
                brokerDetailsWrapper.PagedListingDetails = featuredListings.ToPagedList(pageIndex, pageSize);
            else
                brokerDetailsWrapper.PagedListingDetails = tempInvestorListings.ToPagedList(pageIndex, pageSize);
            if (featuredListings.Count <= 0)
            {
                brokerDetailsWrapper.PagedListingDetails = tempInvestorListings.ToPagedList(pageIndex, pageSize);
            }
            foreach (var item in brokerDetailsWrapper.PagedListingDetails)
            {
                item.ImagePath = CheckFileExists(item.ImagePath, "ListingImagePath", Convert.ToString(item.ID));
            }
            brokerDetailsWrapper.BrokerImage = CheckFileExists(GetUserImage(Convert.ToString(Broker_ID)), "ProfileImagePath", Convert.ToString(Broker_ID),true);
            if (Featured == "0")
            {
                return PartialView("_BrokerListings", brokerDetailsWrapper.PagedListingDetails);
            }
            return View("BrokerDetails", brokerDetailsWrapper);
        }
        #endregion

        #region Get User Image
        public string GetUserImage(string UserID)
        {
            var serialization = new Serialization();
            var investorListingsBA = new InvestorListings();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            HashCriteria.Add("UserID", UserID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = investorListingsBA.GetUserImage(actualCriteria);
            var UserImage = (string)(serialization.DeSerializeBinary(Convert.ToString(result)));
            return UserImage;
        }
        #endregion

        #region Contact Broker
        public ActionResult ContactBroker(string listingId, string Message, string BrokerId)
        {
            var Broker_ID = Convert.ToString(CipherTool.DecryptString(BrokerId));
            var HashCriteria = new Hashtable();
            string actualCriteria;
            var serialization = new Serialization();
            var investorListingsBA = new InvestorListings();
            long listing_ID = 0;
            if (listingId != "0")
            {
                listing_ID = Convert.ToInt64(CipherTool.DecryptString(listingId));
            }
            HashCriteria.Add("ID", listing_ID);
            HashCriteria.Add("Message", Message);
            HashCriteria.Add("Subject", "Contact Broker Request ");
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("IsContactToBroker", true);
            HashCriteria.Add("ReceiverId", Broker_ID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var resultComment = investorListingsBA.SendMessageToBroker(actualCriteria);
            var CommentID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(resultComment)));
            var result = SendContactRequestEmail(CommentID, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Schedule Consultation
        public ActionResult ScheduleConsultation(string listingId, string Email, string Name, string Phone, string Message, string BrokerID)
        {
            var serialization = new Serialization();
            var investorListingsBA = new InvestorListings();
            var HashCriteria = new Hashtable();
            string actualCriteria;
            var listing_ID = Convert.ToInt64(CipherTool.DecryptString(listingId));
            HashCriteria.Add("ID", listing_ID);
            HashCriteria.Add("Message", Message);
            HashCriteria.Add("Subject", "Schedule Consultation Request ");
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("IsContactToBroker", false);
            HashCriteria.Add("EmailAddress", Email);
            HashCriteria.Add("Name", Name);
            HashCriteria.Add("PhoneNumber", Phone);
            HashCriteria.Add("ReceiverId", BrokerID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var resultMessage = investorListingsBA.SendMessageToBroker(actualCriteria);
            var CommentID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(resultMessage)));
            var result = SendContactRequestEmail(CommentID, false);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Send Contact Request Email
        public bool SendContactRequestEmail(long commentID, bool IsContactBroker)
        {
            var serialization = new Serialization();
            var sharedFunctions = new SharedFunctions();
            var emailNotifications = new EmailNotifications();
            var outgoingEmailLogModel = new OutgoingEmailLogModel();
            var HashCriteriaNotification = new Hashtable();
            string actualCriteriaNotification;
            HashCriteriaNotification.Add("ID", commentID);
            actualCriteriaNotification = serialization.SerializeBinary((object)HashCriteriaNotification);
            var resultEmail = emailNotifications.GetNotificationDetails(actualCriteriaNotification);
            outgoingEmailLogModel = (OutgoingEmailLogModel)(serialization.DeSerializeBinary(Convert.ToString(resultEmail)));
            outgoingEmailLogModel.ExternalEmail = System.Configuration.ConfigurationManager.AppSettings["ExternalEmail"];
            //For schedule consulations end email to law
            var scheduleConsulationEmail = System.Configuration.ConfigurationManager.AppSettings["ScheduleConsulationEmail"];
            if (IsContactBroker)
            {
                if (outgoingEmailLogModel.DealName == null || outgoingEmailLogModel.DealName == "")
                {
                    sharedFunctions.SendEmail(outgoingEmailLogModel.EmailTo, Convert.ToString(EmailTemplates.ContactBrokerUnlikeListing), "Synoptek : " + outgoingEmailLogModel.MessageSender + " wants to contact you.", null, outgoingEmailLogModel);
                }
                else
                {
                    sharedFunctions.SendEmail(outgoingEmailLogModel.EmailTo, Convert.ToString(EmailTemplates.ContactBroker), "Synoptek : " + outgoingEmailLogModel.MessageSender + " wants to contact you for - " + outgoingEmailLogModel.DealName, null, outgoingEmailLogModel);
                }
            }
            else
            {
                //Added by Jyotibala P. for public user set Emailto is external address specified in web.config
                if (string.IsNullOrWhiteSpace(outgoingEmailLogModel.EmailTo))
                {
                    if (scheduleConsulationEmail != null && scheduleConsulationEmail != "")
                    {
                        outgoingEmailLogModel.EmailTo = scheduleConsulationEmail;
                    }
                    outgoingEmailLogModel.DealName = "Portal";
                    outgoingEmailLogModel.ReceiverName = "Admin";
                    sharedFunctions.SendEmail(outgoingEmailLogModel.EmailTo, Convert.ToString(EmailTemplates.ScheduleConsultation), "Synoptek : User needs consultation", null, outgoingEmailLogModel);
                }
                else
                {
                    sharedFunctions.SendEmail(outgoingEmailLogModel.EmailTo, Convert.ToString(EmailTemplates.ScheduleConsultation), "Synoptek : " + outgoingEmailLogModel.MessageSender + " needs consultation for - " + outgoingEmailLogModel.DealName, null, outgoingEmailLogModel);
                }
            }
            return true;
        }
        #endregion

        #region View Listing Document
        public ActionResult ViewListingDocument(string listingId)
        {
            var serialization = new Serialization();
            var investorListingsBA = new InvestorListings();
            var investorListingsModel = new InvestorListingsModel();
            var HashCriteria = new Hashtable();
            string actualCriteria;
            var listing_ID = Convert.ToInt64(CipherTool.DecryptString(listingId));
            HashCriteria.Add("ID", listing_ID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = investorListingsBA.GetListingDocuments(actualCriteria);
            var Documents = (List<ListingLoanDocuments>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            var _documentPath = System.Configuration.ConfigurationManager.AppSettings["ListingDocumentPath"];
            foreach (var item in Documents)
            {
                item.FileName = Request.Url.GetLeftPart(UriPartial.Authority) + _documentPath + "//" + listing_ID + "//" + item.DocumentType + "//" + item.FileName;
            }
            return Json(Documents, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}