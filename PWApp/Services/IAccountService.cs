using System.Threading.Tasks;
using PWApp.Entities;

namespace PWApp.Services
{
    public interface IAccountService
    {
        Task<decimal> GetBalance(string userId);

        Task<Account> GetAccount(string userId);

        Task<decimal> Deposit(string userId, decimal amount);

        Task<decimal> Withdraw(string userId, decimal amount);

        void Transfer(string fromUserId, string toUserId, decimal amount);

        Task<Account> CreateAccount(string userId);

        void ActivateAccount(string userId);

        void BlockAccount(string userId);

        Task<bool> IsAccountActive(string userId);
    }
}