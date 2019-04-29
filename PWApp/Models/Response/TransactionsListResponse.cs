using System.Collections.Generic;
using PWApp.EF.Entities;


namespace PWApp.Models.Response
{
    public class TransactionsListResponse : IPaginationResponse<TransactionResponse>
    {
        public List<TransactionResponse> List { get; set; }

        public int TotalCount { get; set; }

        public int? Skipped { get; set; }

        public int? Taken { get; set; }
    }
}