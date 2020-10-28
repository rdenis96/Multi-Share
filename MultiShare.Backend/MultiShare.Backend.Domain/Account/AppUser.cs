using Microsoft.AspNetCore.Identity;

namespace MultiShare.Backend.Domain.Account
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}