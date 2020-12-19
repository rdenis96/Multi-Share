using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MultiShare.Backend.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var tokenDecoded = jwtHandler.ReadToken(token);
            }
        }

        public CustomAuthorizeAttribute(params string[] roles)
            : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}