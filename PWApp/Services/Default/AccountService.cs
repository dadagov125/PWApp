using System;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PWApp.EF;
using PWApp.Entities;

namespace PWApp.Services.Default
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationContext Context;

        private static readonly Mutex
            mutex = new Mutex(true, Assembly.GetExecutingAssembly().GetType().GUID.ToString());

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
            var account = await Context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId);

            if (account == null)
            {
                throw new Exception($"Account for user ${userId} not fount");
            }

            return account;
        }

        public async Task<decimal> Deposit(string userId, decimal amount)
        {
            CheckPositiveAmount(amount);

            Account account = null;

            try
            {
                mutex.WaitOne();

                using (var transaction = await Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        account = await GetAccount(userId);

                        account.Balance += amount;

                        await Context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();

                        throw e;
                    }
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }


            return account.Balance;
        }

        public async Task<decimal> Withdraw(string userId, decimal amount)
        {
            CheckPositiveAmount(amount);

            Account account = null;

            try
            {
                mutex.WaitOne();
                using (var transaction = await Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        account = await GetAccount(userId);

                        account.Balance -= amount;

                        await Context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();

                        throw e;
                    }
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }


            return account.Balance;
        }

        public async Task Transfer(string fromUserId, string toUserId, decimal amount)
        {
            CheckPositiveAmount(amount);

            try
            {
                mutex.WaitOne();
                using (var transaction = await Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var fromAccount = await GetAccount(fromUserId);

                        var toAccount = await GetAccount(toUserId);

                        fromAccount.Balance -= amount;

                        toAccount.Balance += amount;

                        await Context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();

                        throw e;
                    }
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        public async Task<Account> CreateAccount(string userId)
        {
            if ((await Context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId)) != null)
            {
                throw new Exception($"Account for user ${userId} already exists");
            }

            var account = new Account()
            {
                Balance = 0,
                IsActive = true,
                UserId = userId
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

        private void CheckPositiveAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new Exception("Amount cannot be less 0");
            }
        }
    }
}