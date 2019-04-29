using System.Collections.Generic;

namespace PWApp.Models.Response
{
    public class UsersListResponse : IPaginationResponse<UserResponse>
    {
        public List<UserResponse> List { get; set; }

        public int TotalCount { get; set; }

        public int? Skipped { get; set; }

        public int? Taken { get; set; }
    }
}