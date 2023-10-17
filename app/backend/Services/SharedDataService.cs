using backend.Models;
using System.Collections.Generic;
using System;

namespace backend.Services
{
    public class SharedDataService : ISharedDataService
    {
        public SharedDataService(Dictionary<Guid, int> accountBalances)
        {
            AccountBalances = accountBalances;
        }

        public Dictionary<Guid, int> AccountBalances { get; } = new Dictionary<Guid, int>();
        public Dictionary<Guid, Account> Accounts { get; } = new Dictionary<Guid, Account>();
        public Dictionary<Guid, Transaction> Transactions { get; } = new Dictionary<Guid, Transaction>();
    }
}
