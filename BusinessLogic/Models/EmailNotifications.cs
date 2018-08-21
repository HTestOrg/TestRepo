using BusinessObjects;
using BusinessLogic.EmailNotificationServiceReference;
using System.ServiceModel;
using System.Configuration;
using System.Net.Mail;

namespace BusinessLogic.Models
{
    public class EmailNotifications
    {
        #region Common variables
        SmtpClient smtpClientOrg = new SmtpClient();
        EmailNotificationsModel emailNotificationsModel = new EmailNotificationsModel();
        #endregion

        #region Save Outgoing Message Log
        public string SaveOutgoingMessageLog(string emailnotificationCriteria)
        {
            var emailNotificationService = this.GetServiceClient();
            var result = emailNotificationService.InsertIntoOutgoingEmailLog(emailnotificationCriteria);   // Email details inerted into database.
            return result;
        }
        #endregion

        #region Get User Name From Email Address
        public string GetUserNameFromEmailAddress(string emailnotificationCriteria)
        {
            var emailNotificationService = this.GetServiceClient();
            var result = emailNotificationService.GetUserNameFromEmailAddress(emailnotificationCriteria);
            return result;
        }
        #endregion

        #region Get Html Template
        public string GetHtmlForTemplate(string emailnotificationCriteria)
        {
            var emailNotificationService = this.GetServiceClient();
            var result = emailNotificationService.GetHtmlForTemplate(emailnotificationCriteria);
            return result;
        }
        #endregion

        #region Get Notification Details
        public string GetNotificationDetails(string emailnotificationCriteria)
        {
            var emailNotificationService = this.GetServiceClient();
            var result = emailNotificationService.GetNotificationDetails(emailnotificationCriteria);
            return result;
        }
        #endregion

        #region Get Email service reference
        private IEmailNotificationService GetServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var emailNotificationService = ConfigurationManager.AppSettings["EmailNotificationService"];
            EndpointAddress endpoint = new EndpointAddress(emailNotificationService);
            IEmailNotificationService client = ChannelFactory<IEmailNotificationService>.CreateChannel(binding, endpoint);
            return client;
        }
        #endregion
    }
}
