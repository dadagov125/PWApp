using System.ComponentModel.DataAnnotations;

namespace PWApp.Models.Request
{
    public class TransferRequest
    {
        [Required]
        public string ReceiverId { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
    }
}