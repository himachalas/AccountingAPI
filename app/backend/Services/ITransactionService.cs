using backend.Models;
using System.Collections.Generic;
using System;

namespace backend.Services
{
    public interface ITransactionService
    {
        Transaction Deposit(TransactionRequest request);
        Transaction Withdraw(TransactionRequest transactionRequest);
        Transaction GetTransaction(Guid transactionId);
        IEnumerable<Transaction> GetAllTransactions();
        bool TryGetValue(Guid transactionId, out Transaction transaction);
    }
}
