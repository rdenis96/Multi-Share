using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MultiShare.Backend.DataLayer.EntityContext;
using MultiShare.Backend.Domain.Account;
using MultiShare.Backend.Helpers.Common;
using MultiShare.Backend.Helpers.Common.Constants;
using System.Text;

namespace MultiShare.Backend.Extensions
{
    public static class StartupConfigurations
    {
        public static AuthenticationBuilder AddCustomAuthentication(this IServiceCollection services)
        {
            return services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
#if DEBUG
                x.RequireHttpsMetadata = false;
#elif RELEASE
                x.RequireHttpsMetadata = true;
#endif
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    //ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppConfigurationBuilder.Instance.GeneralSettings.JwtSecretKey)),
                    //ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = false
                };
            });
        }

        public static IdentityBuilder AddCustomIdentity(this IServiceCollection services)
        {
            services.AddDbContext<IdentityAppContext>(options =>
            {
                options.UseNpgsql(ConnectionStrings.AuthenticationDataBase, b => b.MigrationsAssembly(CommonConstants.MigrationsAssemblyName));
            });

            return services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
            }).AddEntityFrameworkStores<IdentityAppContext>().AddDefaultTokenProviders();
        }
    }
}