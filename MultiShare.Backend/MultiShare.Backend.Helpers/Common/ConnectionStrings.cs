using Microsoft.Extensions.Configuration;
using MultiShare.Backend.Helpers.Common.Constants;

namespace MultiShare.Backend.Helpers.Common
{
    public static class ConnectionStrings
    {
        public static string AuthenticationDataBase
        {
            get
            {
                return GetConnectionString(ConnectionStringsContextName.AuthenticationContextName);
            }
        }

        public static string RepositoryDataBase
        {
            get
            {
                return GetConnectionString(ConnectionStringsContextName.RepositoryContextName);
            }
        }

        private static string GetConnectionString(string nameOfConnectionString)
        {
            var config = AppConfigurationBuilder.Instance.Config;

            var connectionString = config.GetConnectionString(nameOfConnectionString);
            return connectionString;
        }
    }
}