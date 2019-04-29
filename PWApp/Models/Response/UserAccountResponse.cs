using PWApp.EF.Entities;

namespace PWApp.Models.Response
{
    public class UserAccountResponse : UserResponse
    {
        public decimal Balance { get; set; }


        public static UserAccountResponse FromAccount(Account account)
        {
            if (account == null || account.Owner == null) return null;

            return new UserAccountResponse
            {
                Id = account.Owner.Id,
                Email = account.Owner.Email,
                Phone = account.Owner.PhoneNumber,
                UserName = account.Owner.UserName,
                FirstName = account.Owner.FirstName,
                LastName = account.Owner.LastName,
                Balance = account.Balance
            };
        }
    }
}