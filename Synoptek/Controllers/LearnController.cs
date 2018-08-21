using BusinessLogic.Models;
using BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using PagedList;
using System.Configuration;
using Synoptek.SessionManagement;
using System.Globalization;
using System.Security.Claims;

namespace Synoptek.Controllers
{
    public class LearnController : ParentController
    {
        #region Common variables  
        string userID = Convert.ToString(SessionController.UserSession.UserId);
        private string Education = "1";
        private string Resource = "2";
        #endregion

        #region Index
        public ActionResult Index()
        {
            var actualCriteria = string.Empty;
            var HashCriteria = new Hashtable();
            var serialization = new Serialization();
            var learnHomeModel = new LearnHomeModel();
            var learnBA = new Learn();
            var levels = learnBA.GetExperienceLevel();
            learnHomeModel.ExperienceLevelList = (List<Experiences>)(serialization.DeSerializeBinary(Convert.ToString(levels)));

            HashCriteria.Add("Type", Education);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var topics = learnBA.GetTopics(actualCriteria);
            learnHomeModel.TopicList = (List<Topics>)(serialization.DeSerializeBinary(Convert.ToString(topics)));

            HashCriteria = new Hashtable();
            actualCriteria = string.Empty;
            HashCriteria.Add("Type", Resource);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var resourceTopics = learnBA.GetTopics(actualCriteria);
            learnHomeModel.ResourceTopicList = (List<Topics>)(serialization.DeSerializeBinary(Convert.ToString(resourceTopics)));
            return View(learnHomeModel);
        }
        #endregion

        #region Learn Listing Details
        public ActionResult LearnListingDetails(int? page, LearnWrapper model = null)
        {
            var HashCriteria = new Hashtable();
            var serialization = new Serialization();
            var learnWrapper = new LearnWrapper();
            var learnModel = new LearnModel();
            var learnBA = new Learn();
            var actualCriteria = string.Empty;
            var pageSize = 12;
            var pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            model.SortingOrder = "RecentlyPosted";
            if (model.SelectedExperienceLevel == null && page != null)
            {
                model.SelectedLearnType = Convert.ToString(Session["LearnType"]);
                model.SelectedExperienceLevel = Convert.ToString(Session["ExperienceLevel"]);
                model.SelectedTopic = Convert.ToString(Session["Topic"]);
                model.SearchText = Convert.ToString(Session["SearchText"]);
            }
            else
                RemoveTempDataForLearnListing();

            HashCriteria.Add("ExperienceLevel", model.SelectedExperienceLevel);
            HashCriteria.Add("Topic", model.SelectedTopic);
            HashCriteria.Add("SearchText", model.SearchText);
            HashCriteria.Add("SortingOrder", model.SortingOrder);
            HashCriteria.Add("LearnType", model.SelectedLearnType);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = learnBA.GetLearnListingDetails(actualCriteria);
            learnWrapper.LearnList = (List<LearnModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            learnWrapper.LearnList = SetContentFor(learnWrapper.LearnList);
            learnWrapper.RecentLearnList = GetRecentList(learnWrapper).ToPagedList(pageIndex, pageSize);
            learnWrapper.LearnPagedList = SetDefaultImages(learnWrapper.LearnList.ToPagedList(pageIndex, pageSize));

            var levels = learnBA.GetExperienceLevel();
            learnWrapper.ExperienceLevelList = (List<Experiences>)(serialization.DeSerializeBinary(Convert.ToString(levels)));
            HashCriteria.Add("Type", model.SelectedLearnType);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var topics = learnBA.GetTopics(actualCriteria);
            learnWrapper.TopicList = (List<Topics>)(serialization.DeSerializeBinary(Convert.ToString(topics)));
            var topicname = string.Empty;
            foreach (var ele in learnWrapper.TopicList)
            {
                if (ele.ID == Convert.ToInt32(model.SelectedTopic))
                {
                    topicname = ele.Name;
                }
            }
            ViewBag.TopicName = topicname;
            ViewBag.LearnType = model.SelectedLearnType == "1" ? "Education" : "Resource";
            ViewBag.ExperienceLevel = model.SelectedExperienceLevel == "1" ? "Beginner" : model.SelectedExperienceLevel == "2" ? "Intermediate" : "Advanced";
            return View("LearnListingDetails", learnWrapper);
        }
        #endregion

        #region Get Recent List
        public List<LearnModel> GetRecentList(LearnWrapper learnWrapper)
        {
            var counter = 0;
            var recentLearnResources = new List<LearnModel>();
            foreach (var item in learnWrapper.LearnList)
            {
                if (counter < 4)
                {
                    recentLearnResources.Add(item);
                }
                counter++;
            }
            return recentLearnResources;
        }
        #endregion

        #region Get single resource/education details
        public ActionResult GetSingleLearnDetails(string learnID, string topicID)
        {
            var HashCriteria = new Hashtable();
            var learnModel = new LearnModel();
            var serialization = new Serialization();
            var learnBA = new Learn();
            var learnWrapper = new LearnWrapper();
            var actualCriteria = string.Empty;
            var actualCriteriaLoanDocuments = string.Empty;
            var learn_ID = Convert.ToInt64(CipherTool.DecryptString(learnID, true));
            var topic_ID = Convert.ToInt64(CipherTool.DecryptString(topicID, true));
            var HashCriteriaLoanDocuments = new Hashtable();

            HashCriteria.Add("ID", learn_ID);
            HashCriteria.Add("userID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = learnBA.GetSingleLearnDetails(actualCriteria);
            learnModel = (LearnModel)(serialization.DeSerializeBinary(Convert.ToString(result)));

            //Get learn documents
            HashCriteriaLoanDocuments.Add("UserID", userID);
            HashCriteriaLoanDocuments.Add("ID", learn_ID);
            actualCriteriaLoanDocuments = serialization.SerializeBinary((object)HashCriteriaLoanDocuments);
            var resultDocuments = learnBA.GetLearnDocuments(actualCriteriaLoanDocuments);
            learnModel.DocumentList = (List<Documents>)(serialization.DeSerializeBinary(Convert.ToString(resultDocuments)));

            if (learnModel.ArticleDate != null && learnModel.ArticleDate != "")
            {
                DateTime articleDate = Convert.ToDateTime(learnModel.ArticleDate);
                CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
                learnModel.ArticleDate = articleDate.ToString("D", culture);
            }
            //Get latest 4 articles and videos
            HashCriteria.Add("ExperienceLevel", null);
            HashCriteria.Add("Topic", topic_ID);
            HashCriteria.Add("SearchText", null);
            HashCriteria.Add("SortingOrder", "RecentlyPosted");
            HashCriteria.Add("LearnType", learnModel.LearnTypeID);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var resultResource = learnBA.GetLearnListingDetails(actualCriteria);
            learnWrapper.LearnList = (List<LearnModel>)(serialization.DeSerializeBinary(Convert.ToString(resultResource)));

            foreach (var resource in learnWrapper.LearnList)
            {
                if (resource.ContentForID == (Convert.ToInt32(ContentFor.All)))
                {
                    resource.ContentForName = Convert.ToString(ContentFor.All);
                }
                else if (resource.ContentForID == (Convert.ToInt32(ContentFor.Investor)))
                {
                    resource.ContentForName = Convert.ToString(ContentFor.Investor);

                }
                else if (resource.ContentForID == (Convert.ToInt32(ContentFor.Broker)))
                {
                    resource.ContentForName = Convert.ToString(ContentFor.Broker);
                }
            }

            if (Convert.ToInt32(topic_ID) == 1) // Videos (Webinar)
            {
                learnModel.LatestSimillarArticleList = GetSimillarContentRecentList(learnWrapper, learn_ID);
                //       learnModel.ResourcePath = CheckFileExists(learnModel.ResourcePath, "LearnDocumentPath", Convert.ToString(learnModel.ID)); 
                if (string.IsNullOrEmpty(learnModel.ResourcePath))
                {
                    learnModel.ResourcePath = "";
                }
                else
                {
                    learnModel.ResourcePath = CheckFileExists(learnModel.ResourcePath, "LearnDocumentPath", Convert.ToString(learnModel.ID));
                }
            }
            else if (Convert.ToInt32(topic_ID) == 2) // Articls (Documents)
            {
                learnModel.LatestSimillarArticleList = GetSimillarContentRecentList(learnWrapper, learn_ID);
                foreach (var item in learnModel.LatestSimillarArticleList)
                {
                    if (string.IsNullOrEmpty(item.ResourcePath))
                    {
                        item.ResourcePath = "";
                    }
                    else
                    {
                        item.ResourcePath = CheckFileExists(item.ResourcePath, "LearnImagePath", Convert.ToString(item.ID));
                    }
                }
                if (!string.IsNullOrEmpty(learnModel.ImagePath))
                {
                    learnModel.ResourcePath = CheckFileExists(learnModel.ImagePath, "LearnImagePath", Convert.ToString(learnModel.ID));
                }
                else
                {
                    learnModel.ResourcePath = "";
                }
            }
            else if (Convert.ToInt32(topic_ID) == 12)
            {
                if (!string.IsNullOrEmpty(learnModel.ImagePath))
                {
                    learnModel.ResourcePath = CheckFileExists(learnModel.ImagePath, "LearnImagePath", Convert.ToString(learnModel.ID));
                }
                else
                {
                    learnModel.ResourcePath = "";
                }
            }
            //Get the domain name to share the video on social media
            var domain = Convert.ToString(ConfigurationManager.AppSettings["DomainName"]);

            if (topic_ID == 2 || topic_ID == 9 || topic_ID == 12)
            {
                var webinarLink = domain + "/" + "Learn/GetSingleLearnDetails?LearnID=" + learnID + "%26TopicID=" + topicID;
                TempData["Link"] = webinarLink;
                var description = SharedFunctions.GetPlainTextFromHTML(learnModel.Description);

                TempData["FacebookURL"] = "https://www.facebook.com/sharer.php?u=" + webinarLink;
                ViewBag.LinkedInURL = "https://www.linkedin.com/shareArticle?mini=true&url=" + webinarLink + "&title=" + learnModel.Name + "&summary=" + description;
                ViewBag.TwitterURL = "https://twitter.com/intent/tweet?text=" + learnModel.Name + "&source=" + webinarLink + "&url=" + webinarLink;
                learnModel.TopicID = Convert.ToInt32(topic_ID);
                return View("ArticleDetails", learnModel);
            }
            else
            {
                var webinarLink = domain + "/" + "Learn/GetSingleLearnDetailsForShareFunctionality?LearnID=" + learnID;
                //Do not remove the below test changes
                //learnID = "J9s2323F4HI=";
                //topicID = "KwaSe+MzzuM=";
                //var queryString = "LearnID=" + learnID + "&TopicID=" + topicID;
                //var webinarLink = domain + "/" + "Learn/GetSingleLearnDetails?" + queryString;
                //webinarLink = Uri.EscapeDataString(webinarLink);
                //************** 

                TempData["Link"] = webinarLink;
                var description = SharedFunctions.GetPlainTextFromHTML(learnModel.Description);

                TempData["FacebookURL"] = "https://www.facebook.com/sharer.php?u=" + webinarLink;
                ViewBag.LinkedInURL = "https://www.linkedin.com/shareArticle?mini=true&url=" + webinarLink + "&title=" + learnModel.Name + "&summary=" + description;
                ViewBag.TwitterURL = "https://twitter.com/intent/tweet?text=" + learnModel.Name + "&source=" + webinarLink + "&url=" + webinarLink;
                learnModel.TopicID = Convert.ToInt32(topic_ID);
                return View("WebinarDetails", learnModel);
            }
        }
        #endregion

        #region Get single resource/education details for share
        public ActionResult GetSingleLearnDetailsForShareFunctionality(string learnID)
        {
            var HashCriteria = new Hashtable();
            var learnModel = new LearnModel();
            var serialization = new Serialization();
            var learnBA = new Learn();
            var learnWrapper = new LearnWrapper();
            var actualCriteria = string.Empty;
            var actualCriteriaLoanDocuments = string.Empty;
            var learn_ID = Convert.ToInt64(CipherTool.DecryptString(learnID, true));
            var HashCriteriaLoanDocuments = new Hashtable();

            HashCriteria.Add("ID", learn_ID);
            HashCriteria.Add("userID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = learnBA.GetSingleLearnDetails(actualCriteria);
            learnModel = (LearnModel)(serialization.DeSerializeBinary(Convert.ToString(result)));

            var topic_ID = learnModel.TopicID;
            //Get learn documents
            HashCriteriaLoanDocuments.Add("UserID", userID);
            HashCriteriaLoanDocuments.Add("ID", learn_ID);
            actualCriteriaLoanDocuments = serialization.SerializeBinary((object)HashCriteriaLoanDocuments);
            var resultDocuments = learnBA.GetLearnDocuments(actualCriteriaLoanDocuments);
            learnModel.DocumentList = (List<Documents>)(serialization.DeSerializeBinary(Convert.ToString(resultDocuments)));

            if (learnModel.ArticleDate != null && learnModel.ArticleDate != "")
            {
                DateTime articleDate = Convert.ToDateTime(learnModel.ArticleDate);
                CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
                learnModel.ArticleDate = articleDate.ToString("D", culture);
            }
            //Get latest 4 articles and videos
            HashCriteria.Add("ExperienceLevel", null);
            HashCriteria.Add("Topic", topic_ID);
            HashCriteria.Add("SearchText", null);
            HashCriteria.Add("SortingOrder", "RecentlyPosted");
            HashCriteria.Add("LearnType", learnModel.LearnTypeID);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var resultResource = learnBA.GetLearnListingDetails(actualCriteria);
            learnWrapper.LearnList = (List<LearnModel>)(serialization.DeSerializeBinary(Convert.ToString(resultResource)));

            foreach (var resource in learnWrapper.LearnList)
            {
                if (resource.ContentForID == (Convert.ToInt32(ContentFor.All)))
                {
                    resource.ContentForName = Convert.ToString(ContentFor.All);
                }
                else if (resource.ContentForID == (Convert.ToInt32(ContentFor.Investor)))
                {
                    resource.ContentForName = Convert.ToString(ContentFor.Investor);

                }
                else if (resource.ContentForID == (Convert.ToInt32(ContentFor.Broker)))
                {
                    resource.ContentForName = Convert.ToString(ContentFor.Broker);
                }
            }

            if (Convert.ToInt32(topic_ID) == 1) // Videos (Webinar)
            {
                learnModel.LatestSimillarArticleList = GetSimillarContentRecentList(learnWrapper, learn_ID);
                //       learnModel.ResourcePath = CheckFileExists(learnModel.ResourcePath, "LearnDocumentPath", Convert.ToString(learnModel.ID)); 
                if (string.IsNullOrEmpty(learnModel.ResourcePath))
                {
                    learnModel.ResourcePath = "";
                }
                else
                {
                    learnModel.ResourcePath = CheckFileExists(learnModel.ResourcePath, "LearnDocumentPath", Convert.ToString(learnModel.ID));
                }
            }
            else if (Convert.ToInt32(topic_ID) == 2) // Articls (Documents)
            {
                learnModel.LatestSimillarArticleList = GetSimillarContentRecentList(learnWrapper, learn_ID);
                foreach (var item in learnModel.LatestSimillarArticleList)
                {
                    if (string.IsNullOrEmpty(item.ResourcePath))
                    {
                        item.ResourcePath = "";
                    }
                    else
                    {
                        item.ResourcePath = CheckFileExists(item.ResourcePath, "LearnImagePath", Convert.ToString(item.ID));
                    }
                }
                if (!string.IsNullOrEmpty(learnModel.ImagePath))
                {
                    learnModel.ResourcePath = CheckFileExists(learnModel.ImagePath, "LearnImagePath", Convert.ToString(learnModel.ID));
                }
                else
                {
                    learnModel.ResourcePath = "";
                }
            }
            else if (Convert.ToInt32(topic_ID) == 12)
            {
                if (!string.IsNullOrEmpty(learnModel.ImagePath))
                {
                    learnModel.ResourcePath = CheckFileExists(learnModel.ImagePath, "LearnImagePath", Convert.ToString(learnModel.ID));
                }
                else
                {
                    learnModel.ResourcePath = "";
                }
            }
            //Get the domain name to share the video on social media
            var domain = Convert.ToString(ConfigurationManager.AppSettings["DomainName"]);

            if (topic_ID == 2 || topic_ID == 9 || topic_ID == 12)
            {
                var webinarLink = domain + "/" + "Learn/GetSingleLearnDetails?LearnID=" + learnID;
                TempData["Link"] = webinarLink;
                var description = SharedFunctions.GetPlainTextFromHTML(learnModel.Description);

                TempData["FacebookURL"] = "https://www.facebook.com/sharer.php?u=" + webinarLink;
                ViewBag.LinkedInURL = "https://www.linkedin.com/shareArticle?mini=true&url=" + webinarLink + "&title=" + learnModel.Name + "&summary=" + description;
                ViewBag.TwitterURL = "https://twitter.com/intent/tweet?text=" + learnModel.Name + "&source=" + webinarLink + "&url=" + webinarLink;
                learnModel.TopicID = Convert.ToInt32(topic_ID);
                return View("ArticleDetails", learnModel);
            }
            else
            {
                var webinarLink = domain + "/" + "Learn/GetSingleLearnDetails?LearnID=" + learnID;

                TempData["Link"] = webinarLink;
                var description = SharedFunctions.GetPlainTextFromHTML(learnModel.Description);

                TempData["FacebookURL"] = "https://www.facebook.com/sharer.php?u=" + webinarLink;
                ViewBag.LinkedInURL = "https://www.linkedin.com/shareArticle?mini=true&url=" + webinarLink + "&title=" + learnModel.Name + "&summary=" + description;
                ViewBag.TwitterURL = "https://twitter.com/intent/tweet?text=" + learnModel.Name + "&source=" + webinarLink + "&url=" + webinarLink;
                learnModel.TopicID = Convert.ToInt32(topic_ID);
                return View("WebinarDetails", learnModel);
            }
        }
        #endregion

        #region Get Simillar Content Recent List
        public List<LearnModel> GetSimillarContentRecentList(LearnWrapper learnWrapper, long learnID)
        {
            var counter = 0;
            var recentLearnResources = new List<LearnModel>();
            foreach (var item in learnWrapper.LearnList)
            {
                if (item.ID != Convert.ToInt32(learnID))
                {
                    if (counter < 4)
                    {
                        recentLearnResources.Add(item);
                    }
                    counter++;
                }
            }
            return recentLearnResources;
        }
        #endregion

        #region Edit learn details
        [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin)]
        public ActionResult Edit(string learnID)
        {
            var HashCriteria = new Hashtable();
            var HashCriteriaLoanDocuments = new Hashtable();
            var serialization = new Serialization();
            var learnBA = new Learn();
            var actualCriteria = string.Empty;
            var actualCriteriaLoanDocuments = string.Empty;
            var listing_ID = Convert.ToInt64(CipherTool.DecryptString(learnID, true));
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("ID", Convert.ToInt64(listing_ID));
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);

            var result = learnBA.EditCurrentResource(actualCriteria);
            LearnModel learnEditModel = (LearnModel)(serialization.DeSerializeBinary(Convert.ToString(result)));

            var levels = learnBA.GetExperienceLevel();
            learnEditModel.ExperienceLevelList = (List<Experiences>)(serialization.DeSerializeBinary(Convert.ToString(levels)));

            var LearnTypes = learnBA.GetLearnTypes();
            learnEditModel.LearnTypeList = (List<LearnType>)(serialization.DeSerializeBinary(Convert.ToString(LearnTypes)));

            HashCriteria.Add("Type", learnEditModel.LearnTypeID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);

            var topics = learnBA.GetTopics(actualCriteria);
            learnEditModel.TopicList = (List<Topics>)(serialization.DeSerializeBinary(Convert.ToString(topics)));

            HashCriteriaLoanDocuments.Add("UserID", userID);
            HashCriteriaLoanDocuments.Add("ID", listing_ID);
            actualCriteriaLoanDocuments = serialization.SerializeBinary((object)HashCriteriaLoanDocuments);
            var resultDocuments = learnBA.GetLearnDocuments(actualCriteriaLoanDocuments);
            learnEditModel.DocumentList = (List<Documents>)(serialization.DeSerializeBinary(Convert.ToString(resultDocuments)));

            Session["LearnDocuments"] = learnEditModel.DocumentList;
            Session["ListingsImages"] = learnEditModel.ResourcePath;

            learnEditModel.ResourcePath = CheckFileExists(learnEditModel.ResourcePath, "LearnImagePath", Convert.ToString(listing_ID));

            return View("AddResource", learnEditModel);
        }
        #endregion

        #region Delete listings
        [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin)]
        public ActionResult Delete(string learnID)
        {
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            var learnBA = new Learn();
            var learn_ID = Convert.ToInt64(CipherTool.DecryptString(learnID, true));
            HashCriteria.Add("ID", learn_ID);
            HashCriteria.Add("UserID", userID);
            var actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = learnBA.DeleteCurrentResource(actualCriteria);
            var articleID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
            return Json(articleID, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Remove TempData For Learn Listing
        public ActionResult RemoveTempDataForLearnListing()
        {
            Session["ExperienceLevel"] = "";
            Session["Topic"] = "";
            Session["SearchText"] = "";
            Session["LearnType"] = "";
            return null;
        }
        #endregion 

        #region Set TempData For Learn Filters
        [HttpPost]
        public ActionResult SetTempDataForLearnFilters(LearnWrapper learnWrapper)
        {
            Session["ExperienceLevel"] = learnWrapper.SelectedExperienceLevel;
            Session["Topic"] = learnWrapper.SelectedTopic;
            Session["SearchText"] = learnWrapper.SearchText;
            Session["LearnType"] = learnWrapper.SelectedLearnType;
            return Content("");
        }
        #endregion

        #region Learn Listing Details Partial
        public ActionResult LearnListingDetailsPartial(int? page, string SortingBy = null, LearnWrapper model = null, string ExperienceLevel = null, string SelectedTopic = null, string SearchCriteria = null)
        {
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            var learnListingWrapper = new LearnWrapper();
            var learnBA = new Learn();
            var pageSize = 12;
            var pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            if (Convert.ToString(Session["ExperienceLevel"]) != "")
            {
                model.SelectedExperienceLevel = Convert.ToString(Session["ExperienceLevel"]);
            }
            if (!string.IsNullOrEmpty(ExperienceLevel))
            {
                model.SelectedExperienceLevel = ExperienceLevel;
            }
            if (Convert.ToString(Session["Topic"]) != "")
            {
                model.SelectedTopic = Convert.ToString(Session["Topic"]);
            }
            if (Convert.ToString(Session["SearchText"]) != "")
            {
                model.SearchText = Convert.ToString(Session["SearchText"]);
            }
            if (Convert.ToString(Session["LearnType"]) != "")
            {
                model.SelectedLearnType = Convert.ToString(Session["LearnType"]);
            }
            if (SortingBy != null)
            {
                model.SortingOrder = SortingBy;
            }

            if (!string.IsNullOrEmpty(SearchCriteria))
            {
                model.SearchText = SearchCriteria;
            }
            HashCriteria.Add("ExperienceLevel", model.SelectedExperienceLevel);
            HashCriteria.Add("Topic", model.SelectedTopic);
            HashCriteria.Add("SearchText", model.SearchText);
            HashCriteria.Add("SortingOrder", model.SortingOrder);
            HashCriteria.Add("LearnType", model.SelectedLearnType);
            HashCriteria.Add("UserID", userID);
            var actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = learnBA.GetLearnListingDetails(actualCriteria);
            learnListingWrapper.LearnList = (List<LearnModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            learnListingWrapper.LearnList = SetContentFor(learnListingWrapper.LearnList);
            learnListingWrapper.RecentLearnList = GetRecentList(learnListingWrapper).ToPagedList(pageIndex, pageSize);
            learnListingWrapper.LearnPagedList = SetDefaultImages(learnListingWrapper.LearnList.ToPagedList(pageIndex, pageSize));
            return PartialView("_LearnListings", learnListingWrapper.LearnPagedList);
        }
        #endregion

        #region SetContentFor
        public List<LearnModel> SetContentFor(List<LearnModel> model)
        {
            foreach (var resource in model)
            {
                if (resource.ContentForID == (Convert.ToInt32(ContentFor.All)))
                {
                    resource.ContentForName = Convert.ToString(ContentFor.All);
                }
                else if (resource.ContentForID == (Convert.ToInt32(ContentFor.Investor)))
                {
                    resource.ContentForName = Convert.ToString(ContentFor.Investor);

                }
                else if (resource.ContentForID == (Convert.ToInt32(ContentFor.Broker)))
                {
                    resource.ContentForName = Convert.ToString(ContentFor.Broker);
                }
            }
            return model;
        }
        #endregion

        #region SetDefaultImages
        public IPagedList<LearnModel> SetDefaultImages(IPagedList<LearnModel> model)
        {
            foreach (var item in model)
            {
                if (item.TopicID == 1)
                {
                    item.ResourcePath = "";
                }
                else if (item.TopicID == 2 || item.TopicID == 9)
                {
                    item.ResourcePath = CheckFileExists(item.ResourcePath, "LearnImagePath", Convert.ToString(item.ID));
                }
                else if (item.TopicID == 12)
                {
                    if (!string.IsNullOrEmpty(item.ResourcePath))
                    {
                        item.ResourcePath = CheckFileExists(item.ResourcePath, "LearnImagePath", Convert.ToString(item.ID));
                    }
                    else
                    {
                        item.ResourcePath = "";
                    }
                }
            }
            return model;
        }
        #endregion

        #region Add Resource
        [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin)]
        public ActionResult AddResource()
        {
            var serialization = new Serialization();
            var learnModel = new LearnModel();
            var learnBA = new Learn();
            var HashCriteria = new Hashtable();
            var levels = learnBA.GetExperienceLevel();
            learnModel.ExperienceLevelList = (List<Experiences>)(serialization.DeSerializeBinary(Convert.ToString(levels)));

            var LearnTypes = learnBA.GetLearnTypes();
            learnModel.LearnTypeList = (List<LearnType>)(serialization.DeSerializeBinary(Convert.ToString(LearnTypes)));

            HashCriteria.Add("Type", Education);  // default  Education
            var actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var topics = learnBA.GetTopics(actualCriteria);
            learnModel.TopicList = (List<Topics>)(serialization.DeSerializeBinary(Convert.ToString(topics)));
            learnModel.ContentForID = 3; //default value as All
            return View(learnModel);
        }
        #endregion

        #region Save learn data
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin)]
        public ActionResult SaveLearnData(LearnModel learnModel)
        {
            var HashCriteria = new Hashtable();
            var serialization = new Serialization();
            var learnBA = new Learn();
            var actualCriteria = string.Empty;
            var learnID_New = string.Empty;

            if (ModelState.IsValid)
            {
                HashCriteria.Add("LearnID", learnModel.ID);
                HashCriteria.Add("Name", learnModel.Name);
                HashCriteria.Add("Description", learnModel.Description);
                HashCriteria.Add("Url", learnModel.Url);
                HashCriteria.Add("ContentType", learnModel.ContentTypeID);
                HashCriteria.Add("LearnTypeID", learnModel.LearnTypeID);
                HashCriteria.Add("ExperienceLevelID", learnModel.ExperienceLevelID);
                HashCriteria.Add("ListingStatusID", learnModel.ListingStatusID);
                HashCriteria.Add("TopicID", learnModel.TopicID);
                HashCriteria.Add("IsSponsored", learnModel.IsSponsored);
                HashCriteria.Add("UserID", userID);
                HashCriteria.Add("AuthorName", learnModel.AuthorName);
                HashCriteria.Add("ArticleDate", DateTime.Now);
                HashCriteria.Add("ContentFor", learnModel.ContentForID);
                actualCriteria = serialization.SerializeBinary((object)HashCriteria);

                var result = learnBA.SaveLearnData(actualCriteria);
                var learnID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
                SaveLearnDocuments(Convert.ToInt64(learnID));
                SaveLearnImages(Convert.ToInt64(learnID));
                Session["LearnDocuments"] = null;
                Session["LearnImages"] = null;

                if (learnModel.ID <= 0)
                {
                    TempData["Success"] = "Details has been saved successfully..!";
                    learnID_New = Convert.ToString(learnID);
                }
                else
                {
                    TempData["Success"] = "Details has been modified successfully..!";
                    learnID_New = Convert.ToString(learnModel.ID);
                }

                // Redirect user to single listing page for save and draft option
                //var learn_ID = CipherTool.EncryptString(Convert.ToString(learnID_New), true);
                //var topic_ID = CipherTool.EncryptString(Convert.ToString(learnModel.TopicID), true);

                var resultJson = new { learn_ID = learnID_New, topicID = learnModel.TopicID };

                if (learnModel.LearnStatusID == "3")
                    return Json(resultJson, JsonRequestBehavior.AllowGet);
                //   return Json("/Learn/GetSingleLearnDetails?learnID=" + learn_ID + "&topicID=" + topic_ID, JsonRequestBehavior.AllowGet);
                else
                    if (SessionController.UserSession.RoleType == "Admin")
                {
                    return Json("/Dashboard/Admin", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Learn/Index", JsonRequestBehavior.AllowGet);
            }
            var levels = learnBA.GetExperienceLevel();
            learnModel.ExperienceLevelList = (List<Experiences>)(serialization.DeSerializeBinary(Convert.ToString(levels)));
            var LearnTypes = learnBA.GetLearnTypes();
            learnModel.LearnTypeList = (List<LearnType>)(serialization.DeSerializeBinary(Convert.ToString(LearnTypes)));
            HashCriteria.Add("Type", learnModel.LearnTypeID);
            actualCriteria = string.Empty;
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var topics = learnBA.GetTopics(actualCriteria);
            learnModel.TopicList = (List<Topics>)(serialization.DeSerializeBinary(Convert.ToString(topics)));
            return View("AddResource", learnModel);
        }
        #endregion

        #region Get details to navigate
        public ActionResult GetSingleLearnDetailsForRedirect(string learnID, string topicID)
        {
            var learn_ID = CipherTool.EncryptString(Convert.ToString(learnID), true);
            var topic_ID = CipherTool.EncryptString(Convert.ToString(topicID), true);

            return RedirectToAction("GetSingleLearnDetails", new
            {
                learnID = learn_ID,
                topicID = topic_ID
            });
        }
        #endregion

        #region Save Learn Documents
        public long SaveLearnDocuments(long learnID)
        {
            var actualResult = 0;
            var lstDocuments = Session["LearnDocuments"] as List<Documents>;
            var TempDocumentPath = ConfigurationManager.AppSettings["LearnTempDocumentPath"];
            var DocumentPath = ConfigurationManager.AppSettings["LearnDocumentPath"] + "/" + learnID;
            bool folderExists = Directory.Exists(Server.MapPath(DocumentPath));
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(DocumentPath));

            if (lstDocuments != null && lstDocuments.Count > 0)
            {
                foreach (var tempfile in lstDocuments)
                {
                    var serialization = new Serialization();
                    var learnBA = new Learn();
                    var actualCriteria = string.Empty;
                    var result = string.Empty;
                    var HashCriteria = new Hashtable();
                    var TempserverPath = Server.MapPath(TempDocumentPath + "/" + tempfile.FileName);
                    var ActualImagePath = Server.MapPath(DocumentPath + "/" + tempfile.FileName);
                    var docpresent = System.IO.File.Exists(ActualImagePath);
                    var tempPresent = System.IO.File.Exists(TempserverPath);
                    if (!docpresent && tempPresent)
                    {
                        if (!tempfile.IsDeleted)
                            System.IO.File.Copy(TempserverPath, ActualImagePath);
                    }
                    if (docpresent && tempfile.IsDeleted)
                        System.IO.File.Delete(Server.MapPath(DocumentPath + "/" + tempfile.FileName));

                    //Save Documents in database 
                    HashCriteria.Add("FileName", tempfile.FileName);
                    HashCriteria.Add("IsDeleted", tempfile.IsDeleted);
                    HashCriteria.Add("ID", tempfile.id);
                    HashCriteria.Add("LearnID", learnID);
                    HashCriteria.Add("UserID", userID);
                    actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                    result = Convert.ToString(learnBA.SaveUploadedLearnDocuments(actualCriteria));
                    actualResult = Convert.ToInt32(serialization.DeSerializeBinary(Convert.ToString(result)));
                    System.IO.File.Delete(Server.MapPath(TempDocumentPath + "/" + tempfile.FileName));
                }
            }
            return actualResult;
        }
        #endregion

        #region Save Learn Images
        public void SaveLearnImages(long learnID)
        {
            var TempImagePath = ConfigurationManager.AppSettings["LearnTempImagePath"];
            var ImagePath = ConfigurationManager.AppSettings["LearnImagePath"] + "/" + learnID;
            var lstImages = Session["LearnImages"] as List<Images>;
            var folderExists = Directory.Exists(Server.MapPath(ImagePath));
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(ImagePath));
            if (lstImages != null && lstImages.Count > 0)
            {
                //Delete existing files related to this learn ID
                //string[] existingfiles = System.IO.Directory.GetFiles(Server.MapPath(ImagePath));
                //foreach (string file in existingfiles)
                //{
                //    if (file.Contains("_" + learnID))
                //    {
                //        if (file != Path.Combine(Server.MapPath(ImagePath), lstImages))
                //            System.IO.File.Delete(file);
                //    }
                //} 
                foreach (var tempfile in lstImages)
                {
                    var ActualImagePath = string.Empty;
                    var result = string.Empty;
                    var imageName = string.Empty;
                    var actualCriteria = string.Empty;
                    var serialization = new Serialization();
                    var learnBA = new Learn();
                    var HashCriteria = new Hashtable();
                    var extension = tempfile.FileName.Substring(tempfile.FileName.LastIndexOf('.') + 1);
                    var index = Convert.ToString(tempfile.FileName).IndexOf(".");
                    imageName = tempfile.FileName.Substring(0, index);
                    var TempserverPath = Server.MapPath(TempImagePath + "/" + tempfile.FileName);
                    var filepresentonserver = Server.MapPath(ImagePath + "/" + tempfile.FileName);
                    var imagepresenttrue = System.IO.File.Exists(filepresentonserver);
                    if (!imagepresenttrue)
                    {
                        var filepresent = Server.MapPath(ImagePath + "/" + imageName + "_" + learnID + "." + extension);
                        var imagepresent = System.IO.File.Exists(filepresent);
                        if (!imagepresent)
                        {
                            ActualImagePath = Server.MapPath(ImagePath + "/" + imageName + "_" + learnID + "." + extension);
                            System.IO.File.Copy(TempserverPath, ActualImagePath);
                        }
                        //Update learn image in database
                        var FileName = imageName + "_" + learnID + "." + extension;
                        HashCriteria.Add("FileName", FileName);
                        HashCriteria.Add("ID", tempfile.id);
                        HashCriteria.Add("IsDeleted", tempfile.IsDeleted);
                        HashCriteria.Add("LearnID", learnID);
                        HashCriteria.Add("UserID", userID);
                        actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                        result = learnBA.UpdateLearnImagePath(actualCriteria);
                        result = Convert.ToString(serialization.DeSerializeBinary(Convert.ToString(result)));
                        System.IO.File.Delete(Server.MapPath(TempImagePath + "/" + tempfile.FileName));
                    }
                }
            }
        }
        #endregion

        #region Upload learn documents
        [HttpPost]
        [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin)]
        public ActionResult UploadResourceDocuments(bool IsWebinar = false)
        {
            var uploadFileModel = new List<Documents>();
            var ImagePath = ConfigurationManager.AppSettings["LearnTempDocumentPath"];
            var folderExists = Directory.Exists(Server.MapPath(ImagePath));
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(ImagePath));

            foreach (string file in Request.Files)
            {
                var destinationPath = string.Empty;
                var fileContent = Request.Files[file];
                var photo = Request.Files[file];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    var stream = fileContent.InputStream;
                    var fileName = Path.GetFileName(photo.FileName);
                    var path = Path.Combine(Server.MapPath(ImagePath), fileName);
                    destinationPath = path;
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        stream.CopyTo(fileStream);
                    }
                    if (Session["LearnDocuments"] != null)
                    {
                        var isFileNameRepete = ((List<Documents>)Session["LearnDocuments"]).Find(x => x.FileName == fileName);
                        if (isFileNameRepete == null)
                        {
                            ((List<Documents>)Session["LearnDocuments"]).Add(new Documents { FileName = fileName, IsDeleted = false });
                            return Json("File uploaded successfully");
                        }
                        else
                        {
                            return Json("File is alreday exist");
                        }
                    }
                    else
                    {
                        uploadFileModel.Add(new Documents { FileName = fileName, IsDeleted = false });
                        Session["LearnDocuments"] = uploadFileModel;
                        return Json("File uploaded successfully");
                    }
                }
            }
            return Json("File uploaded successfully");
        }
        #endregion

        #region Remove learn Documents
        [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin)]
        public JsonResult RemoveUploadFile(string fileName, int id)
        {
            var _ImagePath = ConfigurationManager.AppSettings["LearnTempDocumentPath"];
            var serverPath = Server.MapPath(_ImagePath + "/" + fileName.Trim());
            var _ImageActualPath = ConfigurationManager.AppSettings["LearnDocumentPath"] + "//" + id;
            var actualserverPath = Server.MapPath(_ImageActualPath + "/" + fileName.Trim());
            var learnDocuments = (List<Documents>)Session["LearnDocuments"];
            if (learnDocuments != null)
            {
                foreach (var filename in learnDocuments)
                {
                    if (fileName != null || fileName != string.Empty)
                    {
                        var file = new FileInfo(serverPath);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                        var actualfile = new FileInfo(actualserverPath);
                        if (actualfile.Exists)
                        {
                            actualfile.Delete();
                        }
                        var documents = ((List<Documents>)Session["LearnDocuments"]).Find(x => x.FileName == fileName.Trim());
                        documents.IsDeleted = true;
                    }
                }
            }
            return Json("File deleted successfully", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Open learn Documents
        public FileResult OpenFile(string fileName, string LearnID)
        {
            long learnID = Convert.ToInt64(CipherTool.DecryptString(LearnID, true));
            string _ImagePath = System.Configuration.ConfigurationManager.AppSettings["LearnDocumentPath"] + '/' + learnID;
            string serverPath = Server.MapPath(_ImagePath + "/" + fileName);
            return File(new FileStream(serverPath, FileMode.Open), "application/octetstream", fileName);
        }
        #endregion

        #region Upload learn image
        [HttpPost]
        public ActionResult UploadLearnImage()
        {
            var tempImagePath = ConfigurationManager.AppSettings["LearnTempImagePath"];
            var folderExists = Directory.Exists(Server.MapPath(tempImagePath));
            var uploadFileModel = new List<Images>();
            var destinationPath = string.Empty;
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(tempImagePath));

            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];
                var images = Request.Files[file];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    var stream = fileContent.InputStream;
                    var fileName = Path.GetFileName(images.FileName);
                    var path = Path.Combine(Server.MapPath(tempImagePath), fileName);
                    destinationPath = Request.Url.GetLeftPart(UriPartial.Authority) + tempImagePath + "/" + fileName;
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        stream.CopyTo(fileStream);
                    }
                    if (Session["LearnImages"] != null)
                    {
                        var isFileNameRepete = ((List<Images>)Session["LearnImages"]).Find(x => x.FileName == fileName);
                        if (isFileNameRepete == null)
                        {
                            ((List<Images>)Session["LearnImages"]).Add(new Images { FileName = fileName, IsDeleted = false });
                            ViewBag.Message = "File uploaded successfully";
                        }
                        else
                        {
                            ViewBag.Message = "File is already exists";
                        }
                    }
                    else
                    {
                        uploadFileModel.Add(new Images { FileName = fileName, IsDeleted = false });
                        Session["LearnImages"] = uploadFileModel;
                    }
                }
            }
            return Json(destinationPath);
        }
        #endregion

        #region Remove Temp File
        [HttpPost]
        public ActionResult RemoveTempFile(string filename)
        {
            var TempImagePath = ConfigurationManager.AppSettings["LearnTempImagePath"];
            var folderExists = Directory.Exists(Server.MapPath(TempImagePath));
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(TempImagePath));

            string[] existingfiles = Directory.GetFiles(Server.MapPath(TempImagePath));
            foreach (string file in existingfiles)
            {
                if (file.Contains(filename))
                {
                    if (file == Path.Combine(Server.MapPath(TempImagePath), filename))
                        System.IO.File.Delete(file);                    //Delete existing file  
                }
            }
            return Json("File uploaded successfully");
        }
        #endregion

        #region Get topic list according to the resource type
        public ActionResult GetTopicList(int typeID)
        {
            var HashCriteria = new Hashtable();
            var serialization = new Serialization();
            var learnBA = new Learn();
            var actualCriteria = string.Empty;
            HashCriteria.Add("Type", typeID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var topics = learnBA.GetTopics(actualCriteria);    // default  Education
            var result = (List<Topics>)(serialization.DeSerializeBinary(Convert.ToString(topics)));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Post video to Medium
        //Do not remove below code for MEdium Share
        //public ActionResult PostToMedium(string code = "")
        //{
        //    //Get keys from web.config to form url
        //    var clientID = Convert.ToString(ConfigurationManager.AppSettings["mediumClientID"]);
        //    var clientSecret = Convert.ToString(ConfigurationManager.AppSettings["mediumClientSecret"]);

        //    var oAuthClient = new Medium.OAuthClient(clientID, clientSecret);
        //    var MediumUrl = "https://geraciplatform.stage.synoptek.com/Learn/PostToMedium";

        //    //Browser based auth token and get current user details
        //    Medium.Authentication.Token accessToken = oAuthClient.GetAccessToken(code, MediumUrl);
        //    var client = new Medium.Client();
        //    var user = client.GetCurrentUser(accessToken);

        //    //Send the webinar link to the user
        //    var webinarLink = Session["Webinarlink"];

        //    //Create a draft post
        //    var post = client.CreatePost(user.Id, new Medium.Models.CreatePostRequestBody
        //    {
        //        Title = "Webinar Details",
        //        ContentFormat = Medium.Models.ContentFormat.Html,
        //        Tags = new[] { "my post sst", "content", "tag" },
        //        Content = "<a href=" + webinarLink + ">" + webinarLink + "</a>",
        //        PublishStatus = Medium.Models.PublishStatus.Draft,
        //    }, accessToken);

        //    var redirectURL = post.Url;
        //    return Redirect(redirectURL);
        //For google plus
        //ViewBag.GooglePlusURL = "https://plus.google.com/share?url=" + webinarLink + "&gpsrc=gplp0&btmpl=popup#identifier";

        //Medium social network get keys from web.config to form url
        //For medium var webinarMediumLink = domain + "/" + "Learn/GetSingleLearnDetails?LearnID=" + learnID + "&TopicID=" + topicID;
        //Session["Webinarlink"] = webinarMediumLink;
        //var clientID = Convert.ToString(ConfigurationManager.AppSettings["mediumClientID"]);
        //var clientSecret = Convert.ToString(ConfigurationManager.AppSettings["mediumClientSecret"]);
        //var oAuthClient = new Medium.OAuthClient(clientID, clientSecret);
        //var MediumUrl = "https://medium.com/m/oauth/authorize?client_id=9d9ae29409f4&scope=basicProfile,publishPost&state=secretstate&response_type=code&redirect_uri=https://geraciplatform.stage.synoptek.com/Learn/PostToMedium";
        // @ViewBag.MediumURL = MediumUrl;
        //var URL = Request.RawUrl;
        //}
        #endregion 
    }
}