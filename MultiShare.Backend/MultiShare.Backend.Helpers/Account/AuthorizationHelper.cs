using MultiShare.Backend.Domain.Account.Constants;
using MultiShare.Backend.Helpers.Account.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MultiShare.Backend.Helpers.Account
{
    public static class AuthorizationHelper
    {
        public static IEnumerable<Claim> GetDefaultClaimsByType(DefaultClaimsType type)
        {
            var claims = Enumerable.Empty<Claim>();
            switch (type)
            {
                case DefaultClaimsType.Registration:
                    claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, IdentityRoles.User)
                    };
                    break;
            }

            return claims;
        }
    }
}