using System.Collections.Generic;

namespace PWApp.ViewModels
{
    public class UsersListVM : IPaginationResult<UserVM>
    {
        public List<UserVM> List { get; set; }

        public int TotalCount { get; set; }

        public int Skipped { get; set; }

        public int Taken { get; set; }
    }
}