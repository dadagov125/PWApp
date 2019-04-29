using Microsoft.AspNetCore.Identity;

namespace PWApp.EF.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

    }
}