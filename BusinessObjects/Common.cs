
using System;
namespace BusinessObjects
{
    #region Common class for session management
    [Serializable()]
    public class Common
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string RoleType { get; set; }
        public string IsSubscribed { get; set; }
        public Boolean IsEnabled { get; set; }
        public string Password { get; set; }
    }
    #endregion

    #region Exception Handling
    [Serializable()]
    public class CustomAppException
    {
        public string ErrorID { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorSource { get; set; }
        public string StackTrace { get; set; }
    }
    #endregion
}