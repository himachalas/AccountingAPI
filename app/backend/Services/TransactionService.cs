using backend.Enum;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace backend.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly Dictionary<Guid, Transaction> _transactions;
        private readonly Dictionary<Guid, int> _accountBalances;
        private readonly ISharedDataService _sharedDataService;

        public TransactionService(Dictionary<Guid, Transaction> transactions, Dictionary<Guid, int> accountBalances, ISharedDataService sharedDataService)
        {
            _transactions = transactions;
            _accountBalances = accountBalances;
            _sharedDataService = sharedDataService;
        }

        public Transaction CreateTransaction(TransactionRequest transactionRequest)
        {
            try
            {
                if (!_sharedDataService.AccountBalances.TryGetValue(transactionRequest.AccountId, out int currentBalance))
                {
                    throw new InvalidOperationException("Account not found");
                }

                if (transactionRequest.Amount <= 0)
                {
                    throw new ArgumentException("Invalid transaction amount");
                }

                if (transactionRequest.Amount > currentBalance)
                {
                    throw new ArgumentException("Insufficient funds for the transaction");
                }

                var transaction = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    AccountId = transactionRequest.AccountId,
                    Amount = transactionRequest.Amount,
                    CreatedAt = DateTime.UtcNow
                };

                currentBalance += transactionRequest.Amount;
                _sharedDataService.AccountBalances[transactionRequest.AccountId] = currentBalance;

                _sharedDataService.Transactions[transaction.TransactionId] = transaction;
                _transactions[transaction.TransactionId] = transaction;

                return transaction;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Transaction Deposit(TransactionRequest transactionRequest)
        {
            try
            {
                if (!_sharedDataService.AccountBalances.TryGetValue(transactionRequest.AccountId, out int currentBalance))
                {
                    throw new InvalidOperationException("Account not found");
                }

                if (transactionRequest.Amount <= 0)
                {
                    throw new ArgumentException("Invalid transaction amount");
                }

                var transaction = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    AccountId = transactionRequest.AccountId,
                    Amount = transactionRequest.Amount,
                    CreatedAt = DateTime.UtcNow
                };

                currentBalance += transactionRequest.Amount;
                _sharedDataService.AccountBalances[transactionRequest.AccountId] = currentBalance;

                _sharedDataService.Transactions[transaction.TransactionId] = transaction;
                _transactions[transaction.TransactionId] = transaction;

                return transaction;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return _transactions.Values;
        }

        [HttpGet("{transactionId}")]
        public Transaction GetTransaction(Guid transactionId)
        {
            if (_transactions.TryGetValue(transactionId, out var transaction))
            {
                return transaction;
            }

            return null; // Transaction not found
        }

        public bool TryGetValue(Guid transactionId, out Transaction transaction)
        {
            return _transactions.TryGetValue(transactionId, out transaction);
        }

        public Transaction Withdraw(TransactionRequest transactionRequest)
        {
            try
            {
                if (!_sharedDataService.AccountBalances.TryGetValue(transactionRequest.AccountId, out int currentBalance))
                {
                    throw new InvalidOperationException("Account not found");
                }

                if (transactionRequest.Amount <= 0)
                {
                    throw new ArgumentException("Invalid withdrawal amount");
                }

                if (transactionRequest.Amount > currentBalance)
                {
                    throw new ArgumentException("Insufficient funds for the withdrawal");
                }

                var transaction = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    AccountId = transactionRequest.AccountId,
                    Amount = -transactionRequest.Amount,
                    TransactionType = TransactionType.Withdrawal,
                    CreatedAt = DateTime.UtcNow
                };

                currentBalance -= transactionRequest.Amount;
                _sharedDataService.AccountBalances[transactionRequest.AccountId] = currentBalance;

                _transactions[transaction.TransactionId] = transaction;

                return transaction;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
