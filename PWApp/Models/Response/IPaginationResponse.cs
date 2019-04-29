using System.Collections.Generic;

namespace PWApp.Models.Response
{
    public interface IPaginationResponse<T>
    {
        List<T> List { get; set; }

        int TotalCount { get; set; }

        int? Skipped { get; set; }

        int? Taken { get; set; }
    }
}