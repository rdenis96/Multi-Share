using Microsoft.IdentityModel.Tokens;
using MultiShare.Backend.Domain.Account;
using MultiShare.Backend.Helpers.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MultiShare.Backend.BusinessLogic.Account
{
    public class AccountWorker
    {
        public AccountWorker()
        {
        }

        public AppUser GenerateUser(string username, string email)
        {
            AppUser user = new AppUser
            {
                UserName = username,
                Email = email
            };

            return user;
        }

        public string GenerateJwtToken(AppUser user, IList<Claim> userClaims)
        {
            try
            {
                var claims = new[]
                {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }.Union(userClaims);

                var tokenHandler = new JwtSecurityTokenHandler();

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppConfigurationBuilder.Instance.GeneralSettings.JwtSecretKey));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

                var jwtSecurityToken = new JwtSecurityToken(
                    //issuer: AppConfigurationBuilder.Instance.JWTSettings.Issuer, // TO BE ADDED
                    //audience: AppConfigurationBuilder.Instance.JWTSettings.Issuer,
                    claims: claims,
                    signingCredentials: signingCredentials,
                    expires: DateTime.UtcNow.AddMonths(3)
                    );

                var token = tokenHandler.WriteToken(jwtSecurityToken);
                return token;
            }
            catch
            {
                return null;
            }
        }
    }
}