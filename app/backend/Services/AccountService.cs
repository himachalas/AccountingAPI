using backend.Models;
using System.Collections.Generic;
using System;

namespace backend.Services
{
    public class AccountService : IAccountService
    {
        private readonly Dictionary<Guid, Account> _accounts;
        private readonly ISharedDataService _sharedDataService;

        public AccountService(Dictionary<Guid, Account> accounts, ISharedDataService sharedDataService)
        {
            _accounts = accounts;
            _sharedDataService = sharedDataService;
        }

        public AccountService()
        {

        }

        public Account GetAccount(Guid accountId)
        {
            try
            {
                if (_sharedDataService.AccountBalances.TryGetValue(accountId, out int balance))
                {
                    return new Account
                    {
                        AccountId = accountId,
                        Balance = balance
                    };
                }

                return null;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Account UpdateBalance(Guid accountId, BalanceUpdateRequest request)
        {
            try
            {
                if (_sharedDataService.AccountBalances.TryGetValue(accountId, out int currentBalance))
                {

                    if (currentBalance + request.Amount >= 0)
                    {
                        currentBalance += request.Amount;
                        _sharedDataService.AccountBalances[accountId] = currentBalance;

                        return new Account
                        {
                            AccountId = accountId,
                            Balance = currentBalance
                        };
                    }
                    else
                    {
                        throw new InvalidOperationException("Balance cannot go negative.");
                    }
                }

                return null;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void UpdateAccount(Account account)
        {
            try
            {
                if (_sharedDataService.AccountBalances.TryGetValue(account.AccountId, out int currentBalance))
                {
                    _sharedDataService.Accounts[account.AccountId] = account;
                }
                else
                {
                    throw new InvalidOperationException("Account not found");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
