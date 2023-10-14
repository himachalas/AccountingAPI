using backend.Enum;
using System.ComponentModel.DataAnnotations;
using System;

namespace backend.Models
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public int Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}
