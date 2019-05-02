using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PWApp.EF;
using PWApp.EF.Entities;
using PWApp.Models.Filters;
using PWApp.Models.Response;

namespace PWApp.Services.Default
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationContext Context;

        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public AccountService(ApplicationContext context)
        {
            Context = context;
        }

        public async Task<decimal> GetBalance(string userId)
        {
            var account = await GetAccount(userId);

            return account.Balance;
        }

        public async Task<Account> GetAccount(string userId)
        {
            var account = await Context.Accounts.Include(a => a.Owner).FirstOrDefaultAsync(a => a.OwnerId == userId);

            if (account == null)
            {
                throw new Exception($"Account for user ${userId} not fount");
            }

            return account;
        }

        public async Task<TransactionsListResponse> GetTransactions(string userId, PaginationFilter filter)
        {
            TransactionsListResponse result = new TransactionsListResponse();

            var account = await GetAccount(userId);

            var query = Context.Transactions.Where(t => t.ToAccountId == account.Id || t.FromAccountId == account.Id);


            result.TotalCount = await query.CountAsync();

            if (filter != null)
            {
                if (filter.Skip.HasValue)
                {
                    var skip = filter.Skip.Value;
                    query = query.Skip(skip);
                    result.Skipped = skip;
                }

                if (filter.Take.HasValue)
                {
                    var take = filter.Take.Value;
                    query = query.Take(take);
                    result.Taken = take;
                }
            }

            result.List = await query
                .Include(p => p.FromAccount).ThenInclude(a => a.Owner)
                .Include(p => p.ToAccount).ThenInclude(a => a.Owner)
                .OrderBy(t=>t.Created)
                .Select(t => new TransactionResponse
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Created = t.Created,
                    TransactionType = t.TransactionType,
                    Comment = t.Comment,
                    FromUser = UserResponse.FromUser(t.FromAccount.Owner),
                    ToUser = UserResponse.FromUser(t.ToAccount.Owner),
                    Balance = t.Balance
                }).ToListAsync();

            return result;
        }

        public async Task<TransactionResponse> Deposit(string userId, decimal amount)
        {
            CheckPositiveAmount(amount);

            Transaction transaction = null;


            try
            {
                await semaphore.WaitAsync();

                using (var dbContextTransaction = await Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Account account = await GetAccount(userId);

                        account.Balance += amount;

                        transaction = new Transaction()
                        {
                            FromAccountId = account.Id,
                            ToAccountId = null,
                            TransactionType = TransactionType.DEPOSIT,
                            Created = DateTime.Now,
                            Amount = amount,
                            Balance = account.Balance
                        };

                        Context.Transactions.Add(transaction);

                        await Context.SaveChangesAsync();

                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();

                        throw e;
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }


            return TransactionResponse.FromTransaction(transaction);
        }

        public async Task<TransactionResponse> Withdraw(string userId, decimal amount)
        {
            CheckPositiveAmount(amount);

            Transaction transaction = null;

            try
            {
                await semaphore.WaitAsync();
                using (var dbContextTransaction = await Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Account account = await GetAccount(userId);

                        account.Balance -= amount;

                        transaction = new Transaction()
                        {
                            FromAccountId = account.Id,
                            ToAccountId = null,
                            TransactionType = TransactionType.WITHDRAW,
                            Created = DateTime.Now,
                            Amount = amount,
                            Balance = account.Balance
                        };

                        Context.Transactions.Add(transaction);

                        await Context.SaveChangesAsync();

                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();

                        throw e;
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }


            return TransactionResponse.FromTransaction(transaction);
        }

        public async Task<TransactionResponse> Transfer(string fromUserId, string toUserId, decimal amount)
        {
            CheckPositiveAmount(amount);

            Transaction transaction = null;

            try
            {
                await semaphore.WaitAsync();
                using (var dbContextTransaction = await Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var fromAccount = await GetAccount(fromUserId);

                        var toAccount = await GetAccount(toUserId);

                        fromAccount.Balance -= amount;

                        toAccount.Balance += amount;

                        transaction = new Transaction()
                        {
                            FromAccountId = fromAccount.Id,
                            FromAccount = fromAccount,
                            ToAccountId = toAccount.Id,
                            ToAccount = toAccount,
                            TransactionType = TransactionType.TRANSFER,
                            Created = DateTime.Now,
                            Amount = amount,
                            Balance = fromAccount.Balance
                        };

                        Context.Transactions.Add(transaction);

                        await Context.SaveChangesAsync();

                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();

                        throw e;
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }

            return TransactionResponse.FromTransaction(transaction);
        }

        public async Task<Account> OpenAccount(string userId)
        {
            if ((await Context.Accounts.FirstOrDefaultAsync(a => a.OwnerId == userId)) != null)
            {
                throw new Exception($"Account for user ${userId} already exists");
            }

            var account = new Account()
            {
                Balance = 0,
                IsActive = true,
                OwnerId = userId
            };

            Context.Accounts.Add(account);

            await Context.SaveChangesAsync();

            return account;
        }

        public async void ActivateAccount(string userId)
        {
            var account = await GetAccount(userId);

            if (account.IsActive) return;

            account.IsActive = true;

            await Context.SaveChangesAsync();
        }

        public async void BlockAccount(string userId)
        {
            var account = await GetAccount(userId);

            if (!account.IsActive) return;

            account.IsActive = false;

            await Context.SaveChangesAsync();
        }

        public async Task<bool> IsAccountActive(string userId)
        {
            var account = await GetAccount(userId);

            return account.IsActive;
        }

        public async Task<UsersListResponse> GetUsersList(UsersListFilter filter)
        {
            UsersListResponse result = new UsersListResponse();

            var query = Context.Users.AsQueryable();

            result.TotalCount = await query.CountAsync();

            if (filter != null)
            {
                if (filter.Skip.HasValue)
                {
                    var skip = filter.Skip.Value;
                    query = query.Skip(skip);
                    result.Skipped = skip;
                }

                if (filter.Take.HasValue)
                {
                    var take = filter.Take.Value;
                    query = query.Take(take);
                    result.Taken = take;
                }

                if (!string.IsNullOrWhiteSpace(filter.Text))
                {
                    var text = filter.Text;
                    query = query.Where(u =>
                        u.FirstName.Contains(text) ||
                        u.LastName.Contains(text) ||
                        u.UserName.Contains(text) ||
                        u.Email.Contains(text) ||
                        u.PhoneNumber.Contains(text));
                }
            }

            result.List = await query.Select(u => UserResponse.FromUser(u)).ToListAsync();

            return result;
        }

        private void CheckPositiveAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new Exception("Amount cannot be less 0");
            }
        }
    }
}