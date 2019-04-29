using System.Collections.Generic;

namespace PWApp.ViewModels
{
    public interface IPaginationResult<T>
    {
        List<T> List { get; set; }

        int TotalCount { get; set; }

        int? Skipped { get; set; }

        int? Taken { get; set; }
    }
}