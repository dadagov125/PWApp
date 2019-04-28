using System;

namespace PWApp.Entities
{
    public class Account
    {
        public string Id { get; set; }

        public decimal Balance { get; set; }

        public bool IsActive { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }


        public Account()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}