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
    public class AccountController : ControllerBase
    {
        private readonly Dictionary<Guid, int> _accountBalances;
        private readonly IAccountService _accountService;
        private readonly ISharedDataService _sharedDataService;
        public AccountController(Dictionary<Guid, int> accountBalances, IAccountService accountService, ISharedDataService sharedDataService)
        {
            _accountBalances = accountBalances;
            _accountService = accountService;
            _sharedDataService = sharedDataService;
        }

        [HttpGet("account/{accountId}")]
        public ActionResult<Account> GetAccount(Guid accountId)
        {
            try
            {
                if (_sharedDataService.AccountBalances.TryGetValue(accountId, out int balance))
                {
                    var account = new Account
                    {
                        AccountId = accountId,
                        Balance = balance
                    };

                    return Ok(account);
                }

                return NotFound("Account not found");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("accountbalance/{accountId}")]
        public ActionResult<int> GetAccountBalance(Guid accountId)
        {
            try
            {
                if (_sharedDataService.AccountBalances.TryGetValue(accountId, out int balance))
                {
                    return Ok(balance);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }



        [HttpPost]
        public ActionResult<Guid> CreateAccount()
        {
            try
            {
                var accountId = Guid.NewGuid();


                while (_sharedDataService.AccountBalances.ContainsKey(accountId))
                {
                    accountId = Guid.NewGuid();
                }

                var account = new Account
                {
                    AccountId = accountId,
                    Balance = 1000
                };

                _sharedDataService.AccountBalances.Add(accountId, 1000);
                _sharedDataService.Accounts.Add(accountId, account);


                return accountId;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }




        [HttpPatch("{accountId}/update-balance")]
        public ActionResult<Account> UpdateBalance(Guid accountId, BalanceUpdateRequest request)
        {
            if (_sharedDataService.AccountBalances.TryGetValue(accountId, out int currentBalance))
            {

                if (currentBalance + request.Amount >= 0)
                {
                    currentBalance += request.Amount;
                    _sharedDataService.AccountBalances[accountId] = currentBalance;

                    var account = new Account
                    {
                        AccountId = accountId,
                        Balance = currentBalance
                    };


                    _accountService.UpdateAccount(account);

                    return Ok(account);
                }
                else
                {
                    return BadRequest("Balance cannot go negative.");
                }
            }

            return NotFound("Account not found");
        }

    }
}
