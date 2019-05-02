namespace PWApp.Models.Filters
{
    public class UsersListFilter : PaginationFilter
    {
        public string Text { get; set; }

        public string IgnoreUserId { get; set; }
    }
}