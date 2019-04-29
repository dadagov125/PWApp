using System.Collections.Generic;

namespace PWApp.Models.Filters
{
    public class PaginationFilter
    {

        public int? Take { get; set; }

        public int? Skip { get; set; }
    }
}