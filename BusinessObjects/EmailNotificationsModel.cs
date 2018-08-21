using System;

namespace BusinessObjects
{
    [Serializable()]
    public class EmailNotificationsModel
    {
        public string EmailFrom { get; set; }
        public string EmailPassword { get; set; }
        public string SmtpHost { get; set; }
        public bool EnableSSL { get; set; }
        public int PortNumber { get; set; }
        public string EnableEmailSending { get; set; }
    }

    [Serializable()]
    public class OutgoingEmailLogModel
    {
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailSubject { get; set; }
        public string MessageBody { get; set; }
        public string ListingID { get; set; }
        public string CreatedDate { get; set; }
        public string DealName { get; set; }
        public string MessageSender { get; set; }
        public string ExternalEmail { get; set; }
        public string DomainName { get; set; }
        public string Token { get; set; }
        public string ReceiverName { get; set; }
        public string TemplateCode { get; set; }
        public string EmailBody { get; set; }
        public string PhoneNumber { get; set; } 
    }
}