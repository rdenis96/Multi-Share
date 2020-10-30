using MultiShare.Backend.Domain.Account;

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
    }
}