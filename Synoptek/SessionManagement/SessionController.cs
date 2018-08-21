using System.Web;

namespace Synoptek
{
    public static class SessionController
    {
        #region Users
        public class UserSession
        {
            public static string UserId
            {
                get { return (string)HttpContext.Current.Session[SessionDictionary.Users.UserId]; }
                set { HttpContext.Current.Session[SessionDictionary.Users.UserId] = value; }
            }
            public static string UserName
            {
                get { return (string)HttpContext.Current.Session[SessionDictionary.Users.UserName]; }
                set { HttpContext.Current.Session[SessionDictionary.Users.UserName] = value; }
            }
            public static string EmailAddress
            {
                get { return (string)HttpContext.Current.Session[SessionDictionary.Users.EmailAddress]; }
                set { HttpContext.Current.Session[SessionDictionary.Users.EmailAddress] = value; }
            }
            public static string RoleType
            {
                get { return (string)HttpContext.Current.Session[SessionDictionary.Users.RoleType]; }
                set { HttpContext.Current.Session[SessionDictionary.Users.RoleType] = value; }
            }
            public static string IsSubscribed
            {
                get { return (string)HttpContext.Current.Session[SessionDictionary.Users.IsSubscribed]; }
                set { HttpContext.Current.Session[SessionDictionary.Users.IsSubscribed] = value; }
            }
            public static bool IsPaid
            {
                get { return (bool)HttpContext.Current.Session[SessionDictionary.Users.IsPaid]; }
                set { HttpContext.Current.Session[SessionDictionary.Users.IsPaid] = value; }
            }
            public static string CustomerID
            {
                get { return (string)HttpContext.Current.Session[SessionDictionary.Users.CustomerID]; }
                set { HttpContext.Current.Session[SessionDictionary.Users.CustomerID] = value; }
            }
        }
        #endregion
    }
}
