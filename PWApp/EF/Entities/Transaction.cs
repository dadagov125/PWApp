using System;

namespace PWApp.EF.Entities
{
    public class Transaction
    {
        public string Id { get; set; }

        public string FromAccountId { get; set; }

        public Account FromAccount { get; set; }

        public string ToAccountId { get; set; }

        public Account ToAccount { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public DateTime Created { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Comment { get; set; }

        public Transaction()
        {
            Id = Guid.NewGuid().ToString();
        }
    }


    public enum TransactionType : byte
    {
        DEPOSIT=0,
        WITHDRAW=1,
        TRANSFER=3
    }
}