using Microsoft.AspNetCore.Identity;

namespace PWApp.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

    }
}