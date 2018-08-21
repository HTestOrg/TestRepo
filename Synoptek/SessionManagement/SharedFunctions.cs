using BusinessLogic.Models;
using BusinessObjects;
using System;
using System.Collections;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mandrill;

namespace Synoptek.SessionManagement
{
    public class SharedFunctions
    {
        #region Get Email Notifications Model
        public EmailNotificationsModel GetEmailNotificationsModel()
        {
            var emailNotificationsModel = new EmailNotificationsModel();
            emailNotificationsModel.SmtpHost = ConfigurationManager.AppSettings["Host"];
            emailNotificationsModel.EmailFrom = ConfigurationManager.AppSettings["UserName"];
            emailNotificationsModel.EmailPassword = ConfigurationManager.AppSettings["Password"];
            emailNotificationsModel.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
            emailNotificationsModel.PortNumber = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            emailNotificationsModel.EnableEmailSending = Convert.ToString(ConfigurationManager.AppSettings["EnableEmailSending"]);
            return emailNotificationsModel;
        }
        #endregion

        #region Send Email To User
        public bool SendEmail(string emailTo, string emailTemplate, string emailSubject, object model = null, OutgoingEmailLogModel outgoingEmailLogModel = null)
        {
            var emailNotificationBA = new EmailNotifications();
            var emailNotificationsModel = GetEmailNotificationsModel();
            if (outgoingEmailLogModel == null)
            {
                outgoingEmailLogModel = new OutgoingEmailLogModel();
            }
            outgoingEmailLogModel.EmailSubject = emailSubject;
            outgoingEmailLogModel.DomainName = Convert.ToString(ConfigurationManager.AppSettings["DomainName"]);
            SendEmailNotification(emailTo, emailTemplate, model, emailNotificationsModel, outgoingEmailLogModel);
            return true;
        }
        #endregion

        #region Send Email Notifications
        public void SendEmailNotification(string mailTo, string template, Object model = null, EmailNotificationsModel emailNotificationsModel = null, OutgoingEmailLogModel outgoingEmailLogModel = null)
        {
            //System.Web.Mail.MailMessage Msg = new System.Web.Mail.MailMessage();  
            //Msg.From = emailNotificationsModel.EmailFrom; 
            //Msg.To = mailTo;
            //Msg.Bcc = outgoingEmailLogModel.ExternalEmail;
            //Msg.Subject = outgoingEmailLogModel.EmailSubject;
            //Msg.Body = GenerateEmailBody(template, model, outgoingEmailLogModel);
            //// your remote SMTP server IP.
            //System.Web.Mail.SmtpMail.SmtpServer = "67.225.221.112";//your ip address
            //System.Web.Mail.SmtpMail.Send(Msg);

            var emailNotificationBA = new EmailNotifications();
            var serialization = new Serialization();
            var smtpClient = new SmtpClient();
            var smtpClientOrg = new SmtpClient(emailNotificationsModel.SmtpHost);
            smtpClient = smtpClientOrg;
            var mailClient = new MailMessage();
            var HashCriteria = new Hashtable();
            string actualCriteria;

            mailClient.From = new MailAddress(emailNotificationsModel.EmailFrom, "Synoptek");
            mailClient.To.Add(new MailAddress(mailTo));
            List<EmailAddress> mailRecipients = new List<EmailAddress>();
            mailRecipients.Add(new Mandrill.EmailAddress() { email = mailTo });
            mailClient.Subject = outgoingEmailLogModel.EmailSubject;
            if (outgoingEmailLogModel.ExternalEmail != null)
            {
                mailClient.Bcc.Add(new MailAddress(outgoingEmailLogModel.ExternalEmail));
            }
            mailClient.Body = GenerateEmailBody(template, model, outgoingEmailLogModel);
            mailClient.BodyEncoding = System.Text.Encoding.UTF8;
            mailClient.IsBodyHtml = true;

            smtpClientOrg.Credentials = new System.Net.NetworkCredential(emailNotificationsModel.EmailFrom, emailNotificationsModel.EmailPassword);
            smtpClientOrg.EnableSsl = emailNotificationsModel.EnableSSL;
            smtpClientOrg.Port = emailNotificationsModel.PortNumber;
            smtpClientOrg.Host = emailNotificationsModel.SmtpHost;
            smtpClient.UseDefaultCredentials = true;
            if (emailNotificationsModel.EnableEmailSending == "true")
            {
                //SendEmailAsync(mailClient, 1, smtpClient);         // Email sent here ''
                MandrillAPI.SendEmail(mailRecipients, mailClient.Subject, emailNotificationsModel.EmailFrom, "Pixere Team", mailClient.Body);
            }

            var notifications = new OutgoingEmailLogModel();
            notifications.Token = outgoingEmailLogModel.Token;
            if (outgoingEmailLogModel.EmailFrom != null)
            {
                notifications.EmailFrom = outgoingEmailLogModel.EmailFrom;
            }
            else
            {
                notifications.EmailFrom = emailNotificationsModel.EmailFrom;  // if sender is null then email From will be email address from web config.
            }
            HashCriteria.Add("EmailTo", mailTo);
            HashCriteria.Add("EmailBody", mailClient.Body);
            HashCriteria.Add("EmailSubject", outgoingEmailLogModel.EmailSubject);
            HashCriteria.Add("TemplateCode", template);
            HashCriteria.Add("EmailFrom", notifications.EmailFrom);
            HashCriteria.Add("Token", outgoingEmailLogModel.Token);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = emailNotificationBA.SaveOutgoingMessageLog(actualCriteria);
            var emailID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
        }
        #endregion

        #region Generate Email body to send email
        public string GenerateEmailBody(string TemplateName, Object model = null, OutgoingEmailLogModel outgoingEmailLogModel = null)
        {
            var EmailBody = string.Empty;
            var HashCriteria = new Hashtable();
            var HashCriteriaTemplate = new Hashtable();
            var actualCriteria = string.Empty;
            var actualCriteriaTemplate = string.Empty;
            var serialization = new Serialization();
            var emailNotificationBA = new EmailNotifications();
            HashCriteriaTemplate.Add("TemplateCode", TemplateName);
            actualCriteriaTemplate = serialization.SerializeBinary((object)HashCriteriaTemplate);
            var result = Convert.ToString(emailNotificationBA.GetHtmlForTemplate(actualCriteriaTemplate));
            EmailBody = Convert.ToString(serialization.DeSerializeBinary(Convert.ToString(result)));
            EmailBody = EmailBody.Replace("##Year##", Convert.ToString(DateTime.Now.ToString("yyyy")));
            if (!string.IsNullOrEmpty(outgoingEmailLogModel.DomainName))
                EmailBody = EmailBody.Replace("##Domain##", outgoingEmailLogModel.DomainName);

            switch (TemplateName)
            {
                case "ForgotPassword":
                    var forgotPassword = (ForgotPassword)model;
                    var newGuid = Guid.NewGuid();
                    outgoingEmailLogModel.Token = newGuid.ToString();
                    HashCriteria.Add("Email", forgotPassword.Email);
                    actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                    var resultEmail = emailNotificationBA.GetUserNameFromEmailAddress(actualCriteria);  // Get username from email address to display in template
                    var UserName = Convert.ToString(serialization.DeSerializeBinary(Convert.ToString(resultEmail)));
                    if (!string.IsNullOrEmpty(forgotPassword.Email))
                        EmailBody = EmailBody.Replace("##UserName##", UserName);
                    if (!string.IsNullOrEmpty(outgoingEmailLogModel.Token))
                        EmailBody = EmailBody.Replace("##Token##", outgoingEmailLogModel.Token);
                    break;
                case "Registration":
                    var Registration = (UserProfileModel)model;
                    if (!string.IsNullOrEmpty(Registration.Name))
                    {
                        EmailBody = EmailBody.Replace("##UserName##", Registration.Name);
                        EmailBody = EmailBody.Replace("##DiscourseURL##", Convert.ToString(ConfigurationManager.AppSettings["DiscourseUrl"]));
                    }
                        
                    break;
                case "ContactBroker":
                    var contactBroker = outgoingEmailLogModel;
                    if (!string.IsNullOrEmpty(contactBroker.DealName))
                        EmailBody = EmailBody.Replace("##DealName##", contactBroker.DealName);
                    if (!string.IsNullOrEmpty(contactBroker.ReceiverName))
                        EmailBody = EmailBody.Replace("##BrokerName##", contactBroker.ReceiverName);
                    if (!string.IsNullOrEmpty(contactBroker.MessageBody))
                        EmailBody = EmailBody.Replace("##Message##", contactBroker.MessageBody);
                    if (!string.IsNullOrEmpty(contactBroker.MessageSender))
                        EmailBody = EmailBody.Replace("##MessageSender##", contactBroker.MessageSender);
                    break;
                case "ScheduleConsultation":
                    var scheduleConsultation = outgoingEmailLogModel;
                    if (!string.IsNullOrEmpty(scheduleConsultation.DealName))
                        EmailBody = EmailBody.Replace("##DealName##", scheduleConsultation.DealName);
                    if (!string.IsNullOrEmpty(scheduleConsultation.ReceiverName))
                        EmailBody = EmailBody.Replace("##BrokerName##", scheduleConsultation.ReceiverName);
                    if (!string.IsNullOrEmpty(scheduleConsultation.MessageBody))
                        EmailBody = EmailBody.Replace("##Message##", scheduleConsultation.MessageBody);
                    if (!string.IsNullOrEmpty(scheduleConsultation.MessageSender))
                        EmailBody = EmailBody.Replace("##MessageSender##", scheduleConsultation.MessageSender);
                    if (!string.IsNullOrEmpty(scheduleConsultation.EmailTo))
                        EmailBody = EmailBody.Replace("##Email##", scheduleConsultation.EmailFrom);
                    if (!string.IsNullOrEmpty(scheduleConsultation.PhoneNumber))
                        EmailBody = EmailBody.Replace("##PhoneNumber##", scheduleConsultation.PhoneNumber);
                    else
                        EmailBody = EmailBody.Replace("##PhoneNumber##", "Not Provided");
                    break;
                case "MessageCenter":
                    var MessageCenter = outgoingEmailLogModel;
                    if (!string.IsNullOrEmpty(MessageCenter.DealName))
                        EmailBody = EmailBody.Replace("##DealName##", MessageCenter.DealName);
                    if (!string.IsNullOrEmpty(MessageCenter.ReceiverName))
                        EmailBody = EmailBody.Replace("##ReceiverName##", MessageCenter.ReceiverName);
                    if (!string.IsNullOrEmpty(MessageCenter.MessageBody))
                        EmailBody = EmailBody.Replace("##Message##", MessageCenter.MessageBody);
                    if (!string.IsNullOrEmpty(MessageCenter.MessageSender))
                        EmailBody = EmailBody.Replace("##MessageSender##", MessageCenter.MessageSender);
                    break;
                case "ContactBrokerUnlikeListing":
                    var contactBrokerUnlikeListing = outgoingEmailLogModel;
                    if (!string.IsNullOrEmpty(contactBrokerUnlikeListing.ReceiverName))
                        EmailBody = EmailBody.Replace("##BrokerName##", contactBrokerUnlikeListing.ReceiverName);
                    if (!string.IsNullOrEmpty(contactBrokerUnlikeListing.MessageBody))
                        EmailBody = EmailBody.Replace("##Message##", contactBrokerUnlikeListing.MessageBody);
                    if (!string.IsNullOrEmpty(contactBrokerUnlikeListing.MessageSender))
                        EmailBody = EmailBody.Replace("##MessageSender##", contactBrokerUnlikeListing.MessageSender);
                    break;
            }
            return EmailBody;
        }
        #endregion

        #region Send Email
        private static async Task SendEmailAsync(MailMessage ObjMailMessage, int NotificationID, SmtpClient smtpClient)
        {
            try
            {
                await Task.Run(() =>
                {
                    smtpClient.Send(ObjMailMessage);
                });
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion

        #region Common function to get the user's role and also user is paid or not  
        public List<UserProfileEditModel> GetUserRole(long? userID)
        {
            Serialization serialization = new Serialization();
            UserRegistration userRegistrationBA = new UserRegistration();
            List<UserProfileEditModel> userProfileModel = new List<UserProfileEditModel>();
            Hashtable HashCriteria = new Hashtable();
            string actualCriteria;
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = userRegistrationBA.GetUserSpecificDetails(actualCriteria);
            userProfileModel = (List<UserProfileEditModel>)(serialization.DeSerializeBinary(Convert.ToString(result)));
            return userProfileModel;
        }
        #endregion  

        #region Get Duration method
        public static string GetDuration(string selctedDate)
        {
            var duration = string.Empty;
            var currentdate = DateTime.Now;
            var oldDate = Convert.ToDateTime(selctedDate);
            TimeSpan span = currentdate.Subtract(oldDate);
            if (span.Days == 1)
                duration = string.Format("{0} day ago", span.Days);
            else if (span.Days > 1)
                duration = string.Format("{0} days ago", span.Days);
            else if (span.Hours > 0 && span.Days < 1)
                duration = string.Format("{0} hours ago", span.Hours);
            else if (span.Hours <= 0 && span.Minutes > 0)
                duration = string.Format("{0} minutes ago", span.Minutes);
            else if (span.Minutes <= 0)
                duration = string.Format("{0} seconds ago", span.Seconds);
            return duration;
        }
        #endregion

        #region Get plain text without HTML tags
        public static string GetPlainTextFromHTML(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
        #endregion

        public class MandrillAPI
        {
            private string _apiKey;
            public MandrillAPI(string apiKey)
            {
                _apiKey = apiKey;
            }
            /// <summary>
            /// Used to set mandrill setting and send an email
            /// </summary>
            /// <param name="emailAddress"></param>
            /// <param name="subject"></param>
            /// <param name="fromEmail"></param>
            /// <param name="fromName"></param>
            /// <param name="content"></param>
            public static void SendEmail(List<EmailAddress> emailAddress, string subject, string fromEmail, string fromName,
               string content)
            {
                var apiKey = ConfigurationManager.AppSettings["MandrillKey"];
                var unSubCallback = ConfigurationManager.AppSettings["MandrillUnSubCallback"];
                var api = new MandrillApi(apiKey);
                var message = content;// +"\n" +

                var result = api.SendMessage(new EmailMessage
                {
                    to = emailAddress,
                    from_email = fromEmail,
                    from_name = fromName,
                    subject = subject,
                    html = message,
                    preserve_recipients = false,
                    track_opens = true,
                    track_clicks = true,
                });
            }
        }
    }
}


