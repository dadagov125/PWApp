using PWApp.Entities;

namespace PWApp.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public static UserVM FromUser(User user)
        {
            if (user == null) return null;
            return new UserVM
            {
                Id = user.Id,
                Email = user.Email,
                Phone = user.PhoneNumber,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}