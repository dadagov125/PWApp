
using System.Threading.Tasks;
using PWApp.EF.Entities;
using PWApp.Models.Filters;
using PWApp.Models.Response;


namespace PWApp.Services
{
    public interface IAccountService
    {
        Task<decimal> GetBalance(string userId);

        Task<Account> GetAccount(string userId);

        Task<TransactionsListResponse> GetTransactions(string userId, PaginationFilter filter);

        Task<Transaction> Deposit(string userId, decimal amount);

        Task<Transaction> Withdraw(string userId, decimal amount);

        Task<Transaction> Transfer(string fromUserId, string toUserId, decimal amount);

        Task<Account> OpenAccount(string userId);

        void ActivateAccount(string userId);

        void BlockAccount(string userId);

        Task<bool> IsAccountActive(string userId);

        Task<UsersListResponse> GetUsersList(UsersListFilter filter);
    }
}