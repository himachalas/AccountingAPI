using System;

namespace backend.Models
{
    public class WithdrawRequest
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
