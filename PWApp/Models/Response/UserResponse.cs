using PWApp.EF.Entities;

namespace PWApp.Models.Response
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public static UserResponse FromUser(User user)
        {
            if (user == null) return null;
            return new UserResponse
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