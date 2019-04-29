using System.ComponentModel.DataAnnotations;

namespace PWApp.ViewModels
{
    public class TransferVN
    {
        [Required]
        public string ReceiverId { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
    }
}