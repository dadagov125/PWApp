using System;
using PWApp.EF.Entities;

namespace PWApp.Models.Response
{
    public class TransactionResponse
    {
        public string Id { get; set; }

        public UserResponse FromUser { get; set; }

        public UserResponse ToUser { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public DateTime Created { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Comment { get; set; }
    }
}