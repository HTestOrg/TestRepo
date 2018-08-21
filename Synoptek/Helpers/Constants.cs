using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Synoptek.Helpers
{
    public static class Constants
    {
        
        public static class UserRoles
        {
            public const string Admin = "Admin";
            public const string Investor = "Investor";
            public const string Broker = "Broker";
            public const string RegisterUser = "RegisterUser";
        }

        internal const string UserSession = "UserSession";

        internal const string Issuer = "https://synoptek.com";
    }
}