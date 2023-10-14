using backend.Models;
using System;

namespace backend.Services
{
    public interface IAccountService
    {
        Account GetAccount(Guid accountId);
        Account UpdateBalance(Guid accountId, BalanceUpdateRequest request);
        void UpdateAccount(Account account);
    }
}
