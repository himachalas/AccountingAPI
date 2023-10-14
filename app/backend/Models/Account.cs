using System.ComponentModel.DataAnnotations;
using System;

namespace backend.Models
{
    public class Account
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public int Balance { get; set; }
    }
}
