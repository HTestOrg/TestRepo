using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessObjects;
using PagedList;
using Synoptek.SessionManagement;
using BusinessLogic.Models;
using Synoptek.Enums;
using System.Security.Claims;

namespace Synoptek.Controllers
{
    [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Broker + "," + Helpers.Constants.UserRoles.Investor + "," + Helpers.Constants.UserRoles.Admin)]
    public class MessageCenterController : BaseController
    {
        #region Common variables
        string userID = Convert.ToString(SessionController.UserSession.UserId);
        #endregion 
        
        #region Get User Messages
        public ActionResult Inbox(int? page, string Read = null)
        {
            var brokerListings = new BrokerListings();
            var serialization = new Serialization();
            var messageCenterWrapper = new MessageCenterWrapper();
            var HashCriteria = new Hashtable();
            string actualCriteria;
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = brokerListings.GetUserMessage(actualCriteria);
            List<MessageCenterModel> lstmessageCenter = (List<MessageCenterModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            messageCenterWrapper.MessageAll = new List<MessageCenterModel>();
            messageCenterWrapper.MessageArchived = new List<MessageCenterModel>();
            messageCenterWrapper.MessageUnread = new List<MessageCenterModel>();
            foreach (var item in lstmessageCenter)
            {
                item.Duration = SharedFunctions.GetDuration(item.Duration);

                if (item.IsArchived)
                {
                    messageCenterWrapper.MessageArchived.Add(item);
                }
                else
                {
                    messageCenterWrapper.MessageAll.Add(item);
                }
                if (!item.IsRead && !item.IsArchived)
                {
                    messageCenterWrapper.MessageUnread.Add(item);
                }
                //switch (item.IsRead)
                //{
                //    case true:
                //        messageCenterWrapper.MessageRead.Add(item);
                //        messageCenterWrapper.MessageAll.Add(item);
                //        break;

                //    case false:
                //        messageCenterWrapper.MessageUnread.Add(item);
                //        messageCenterWrapper.MessageAll.Add(item);
                //        break;
                //}
            }
            int pageSize = 8;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            messageCenterWrapper.PagedMessageAll = messageCenterWrapper.MessageAll.ToPagedList(pageIndex, pageSize);
            messageCenterWrapper.PagedMessageUnread = messageCenterWrapper.MessageUnread.ToPagedList(pageIndex, pageSize);
            messageCenterWrapper.PagedMessageArchived = messageCenterWrapper.MessageArchived.ToPagedList(pageIndex, pageSize);

            if (Read == "0" || Read == null)
            {
                foreach (var item in messageCenterWrapper.PagedMessageUnread)
                {
                    item.ProfileImage = CheckFileExists(item.ProfileImage, "ProfileImagePath", Convert.ToString(item.Sender), true);
                    item.MessageStatus = "0";
                }
            }
            if (Read == "All")
            {
                foreach (var item in messageCenterWrapper.PagedMessageAll)
                {
                    item.ProfileImage = CheckFileExists(item.ProfileImage, "ProfileImagePath", Convert.ToString(item.Sender), true);
                    item.MessageStatus = "All";
                }
                return PartialView("_MyInbox", messageCenterWrapper.PagedMessageAll);
            }
            if (Read == "0")
            {   
                return PartialView("_MyInbox", messageCenterWrapper.PagedMessageUnread);
            }

            if (Read == "1")
            {
                foreach (var item in messageCenterWrapper.PagedMessageArchived)
                {
                    item.ProfileImage = CheckFileExists(item.ProfileImage, "ProfileImagePath", Convert.ToString(item.Sender), true);
                    item.MessageStatus = "1";
                }
                return PartialView("_MyInbox", messageCenterWrapper.PagedMessageArchived);
            }
            return View("Inbox", messageCenterWrapper);
        }
        #endregion

        #region Message center Reply to message
        public ActionResult ReplyToMessage(string listingDetailsID, string toUserId, string message, string CommentID = null)
        {
            var brokerListings = new BrokerListings();
            var serialization = new Serialization();
            var sharedFunctions = new SharedFunctions();
            var emailNotifications = new EmailNotifications();
            var outgoingEmailLogModel = new OutgoingEmailLogModel();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var CommentSubject = " You got reply ";
            HashCriteria.Add("ID", listingDetailsID);
            HashCriteria.Add("ToUserID", toUserId);
            HashCriteria.Add("Message", message);
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("ReplyToComment", CommentID);
            HashCriteria.Add("CommentSubject", CommentSubject);
            HashCriteria.Add("IsContactToBroker", true);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria); 
            var result = Convert.ToString(brokerListings.ReplyToMessage(actualCriteria));
            var commentID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));

            HashCriteria = new Hashtable();
            actualCriteria = string.Empty;
            HashCriteria.Add("ID", commentID);
            actualCriteria = Convert.ToString(serialization.SerializeBinary((object)HashCriteria)); 
            var resultEmail = Convert.ToString(emailNotifications.GetNotificationDetails(actualCriteria));
            outgoingEmailLogModel = (OutgoingEmailLogModel)(serialization.DeSerializeBinary(Convert.ToString(resultEmail))); 
            outgoingEmailLogModel.ExternalEmail = System.Configuration.ConfigurationManager.AppSettings["ExternalEmail"];
            var response = sharedFunctions.SendEmail(outgoingEmailLogModel.EmailTo, Convert.ToString(EmailTemplates.MessageCenter), "Synoptek : " + outgoingEmailLogModel.MessageSender + " sent you message on - " + outgoingEmailLogModel.DealName, null, outgoingEmailLogModel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region Archive Message
        public ActionResult ArchiveMessage(string messageID)
        {
            var brokerListings = new BrokerListings();
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            HashCriteria.Add("ID", messageID);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = brokerListings.ArchiveCurrentMessage(actualCriteria);
            var messageId = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
            return Json(messageId, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //#region Delete Message
        //public ActionResult DeleteMessage(string messageID)
        //{
        //    var brokerListings = new BrokerListings();
        //    var serialization = new Serialization();
        //    var HashCriteria = new Hashtable();
        //    var actualCriteria = string.Empty;
        //    HashCriteria.Add("ID", messageID);
        //    HashCriteria.Add("UserID", userID);
        //    actualCriteria = serialization.SerializeBinary((object)HashCriteria);
        //    var result = brokerListings.DeleteCurrentMessage(actualCriteria);
        //    var messageId = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        #region Get unread message count
        public ActionResult GetUnreadMessageCount()
        {
            var brokerListings = new BrokerListings();
            var serialization = new Serialization();
            var messageCenterWrapper = new MessageCenterWrapper();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty; 
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria); 
            var result = brokerListings.GetUnreadMessages(actualCriteria);
            messageCenterWrapper.MessageUnread = (List<MessageCenterModel>)(serialization.DeSerializeBinary(Convert.ToString(result))); 
            var count = messageCenterWrapper.MessageUnread.Where(o => (o.IsRead == false)).ToList().Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get unread messages
        public ActionResult GetUnreadMessages()
        {
            var brokerListings = new BrokerListings();
            var serialization = new Serialization();
            var messageCenterWrapper = new MessageCenterWrapper();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty; 
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria); 
            var result = brokerListings.GetUnreadMessages(actualCriteria);
            messageCenterWrapper.MessageUnread = (List<MessageCenterModel>)(serialization.DeSerializeBinary(Convert.ToString(result))); 
            foreach (var item in messageCenterWrapper.MessageUnread)
            {
                item.Duration = SharedFunctions.GetDuration(item.Duration);
                item.ProfileImage = CheckFileExists(item.ProfileImage, "ProfileImagePath", Convert.ToString(item.Sender), true);
            }
            return PartialView("_NotificationMessages", messageCenterWrapper.MessageUnread.Where(o => (o.IsRead == false)).ToList());
        }
        #endregion 

        #region Mark Message As Read
        public ActionResult MarkAsReadMessage(string messageID)
        {
            var brokerListings = new BrokerListings();
            var serialization = new Serialization();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            HashCriteria.Add("ID", messageID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = brokerListings.MarkAsReadMessage(actualCriteria);
            result = Convert.ToString(serialization.DeSerializeBinary(Convert.ToString(result)));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}