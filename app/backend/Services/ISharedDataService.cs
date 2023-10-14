using backend.Models;
using System.Collections.Generic;
using System;

namespace backend.Services
{
    public interface ISharedDataService
    {
        Dictionary<Guid, int> AccountBalances { get; }
        Dictionary<Guid, Account> Accounts { get; }
        Dictionary<Guid, Transaction> Transactions { get; }
    }
}
