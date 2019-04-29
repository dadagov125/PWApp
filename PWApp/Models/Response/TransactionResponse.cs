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

        public static TransactionResponse FromTransaction(Transaction transaction)
        {
            if (transaction == null) return null;
            return new TransactionResponse
            {
                Id = transaction.Id,
                FromUser = UserResponse.FromUser(transaction.FromAccount?.Owner),
                ToUser = UserResponse.FromUser(transaction.ToAccount?.Owner),
                Amount = transaction.Amount,
                Balance = transaction.Balance,
                Created = transaction.Created,
                TransactionType = transaction.TransactionType,
                Comment = transaction.Comment
            };
        }
    }
}