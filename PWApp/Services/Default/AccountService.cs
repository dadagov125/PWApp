using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PWApp.EF;
using PWApp.Entities;
using PWApp.ViewModels;

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
            var account = await Context.Accounts.FirstOrDefaultAsync(a => a.OwnerId == userId);

            if (account == null)
            {
                throw new Exception($"Account for user ${userId} not fount");
            }

            return account;
        }

        public async Task<TransactionsListVM> GetTransactions(string userId, QueryFilter filter)
        {
            TransactionsListVM result = new TransactionsListVM();

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

            result.Transactions = await query.ToListAsync();

            return result;
        }

        public async Task<Transaction> Deposit(string userId, decimal amount)
        {
            CheckPositiveAmount(amount);

            Transaction transaction = null;


            try
            {
                mutex.WaitOne();

                using (var dbContextTransaction = await Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Account account = await GetAccount(userId);

                        account.Balance += amount;


                        Context.Transactions.Add(new Transaction()
                        {
                            FromAccountId = account.Id,
                            ToAccountId = null,
                            TransactionType = TransactionType.DEPOSIT,
                            Created = DateTime.Now,
                            Amount = amount,
                            Balance = account.Balance
                        });

                        await Context.SaveChangesAsync();
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
                mutex.ReleaseMutex();
            }


            return transaction;
        }

        public async Task<Transaction> Withdraw(string userId, decimal amount)
        {
            CheckPositiveAmount(amount);

            Transaction transaction = null;

            try
            {
                mutex.WaitOne();
                using (var dbContextTransaction = await Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Account account = await GetAccount(userId);

                        account.Balance -= amount;

                        Context.Transactions.Add(new Transaction()
                        {
                            FromAccountId = account.Id,
                            ToAccountId = null,
                            TransactionType = TransactionType.WITHDRAW,
                            Created = DateTime.Now,
                            Amount = amount,
                            Balance = account.Balance
                        });

                        await Context.SaveChangesAsync();
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
                mutex.ReleaseMutex();
            }


            return transaction;
        }

        public async Task<Transaction> Transfer(string fromUserId, string toUserId, decimal amount)
        {
            CheckPositiveAmount(amount);

            Transaction transaction = null;

            try
            {
                mutex.WaitOne();
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
                            ToAccountId = toAccount.Id,
                            TransactionType = TransactionType.TRANSFER,
                            Created = DateTime.Now,
                            Amount = amount,
                            Balance = fromAccount.Balance
                        };

                        Context.Transactions.Add(transaction);

                        await Context.SaveChangesAsync();
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
                mutex.ReleaseMutex();
            }

            return transaction;
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

        private void CheckPositiveAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new Exception("Amount cannot be less 0");
            }
        }
    }
}