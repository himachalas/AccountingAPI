using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace backend.Models
{
    public class DepositRequest
    {
         public Guid AccountId { get; set; } 
         public decimal Amount { get; set; } 
    }
}
