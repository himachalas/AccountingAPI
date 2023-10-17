using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ISharedDataService _sharedDataService;
        public TransactionsController(ITransactionService transactionService, ISharedDataService sharedDataService)
        {
            _transactionService = transactionService;
            _sharedDataService = sharedDataService;
        }

        [HttpPost("deposit")]
        public ActionResult<Transaction> Deposit([FromBody] TransactionRequest transactionRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var transaction = _transactionService.Deposit(transactionRequest);
                return CreatedAtAction(nameof(GetTransaction), new { transactionId = transaction.TransactionId }, transaction);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        public ActionResult<IEnumerable<Transaction>> GetTransactions()
        {
            var transactions = _transactionService.GetAllTransactions();
            return Ok(transactions);
        }

        [HttpGet("{transactionId}")]
        public IActionResult GetTransaction(Guid transactionId)
        {
            if (_transactionService.TryGetValue(transactionId, out var transaction))
            {
                return Ok(transaction);
            }

            return NotFound();
        }

        [HttpPatch("withdraw")]
        public ActionResult<Transaction> Withdraw([FromBody] TransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var withdrawalTransaction = _transactionService.Withdraw(request);

                return CreatedAtAction(nameof(GetTransaction), new { transactionId = withdrawalTransaction.TransactionId }, withdrawalTransaction);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
