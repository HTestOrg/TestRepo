using BusinessObjects;
using System.Collections.Generic;
using System.Security.Claims;

namespace Synoptek.Helpers
{
    internal class AuthenticationHelper
    {
        internal static List<Claim> CreateClaim(UserAuthenticationModel userAuthModel, params string[] roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userAuthModel.UserId.ToString()),
                new Claim(ClaimTypes.Name, userAuthModel.DisplayName),
                new Claim(Constants.UserSession, userAuthModel.ToJson())
            };

            //for multiple roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, Constants.Issuer));
            }
            return claims;
        }
    }
}