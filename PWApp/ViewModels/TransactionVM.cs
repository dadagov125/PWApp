using System;
using PWApp.Entities;

namespace PWApp.ViewModels
{
    public class TransactionVM
    {
        public string Id { get; set; }

        public UserVM FromUser { get; set; }

        public UserVM ToUser { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public DateTime Created { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Comment { get; set; }
    }
}