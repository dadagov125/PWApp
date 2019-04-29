using System.Collections.Generic;
using PWApp.Entities;


namespace PWApp.ViewModels
{
    public class TransactionsListVM
    {
        public List<Transaction> Transactions { get; set; }

        public int TotalCount { get; set; }

        public int Skipped { get; set; }

        public int Taken { get; set; }
    }
}