using System;
using System.Collections.Generic;

namespace PWApp.Entities
{
    public class Account
    {
        public string Id { get; set; }

        public decimal Balance { get; set; }

        public bool IsActive { get; set; }

        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public List<Transaction> Transactions;


        public Account()
        {
            Id = Guid.NewGuid().ToString();

            Transactions = new List<Transaction>();
        }
    }
}