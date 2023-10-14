using System.ComponentModel.DataAnnotations;
using System;

namespace backend.Models
{
    public class TransactionRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public int Amount { get; set; }
    }
}
