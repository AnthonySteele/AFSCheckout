using System.ComponentModel.DataAnnotations;

namespace CheckoutApi.Controllers
{
    public class PaymentRequest
    {
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string NameOnCard { get; set; } = string.Empty;

        [Required]
        [CreditCard]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string CardCvv { get; set; } = string.Empty;

        [Required]
        [Range(1, 12)]
        public int CardExpiryMonth { get; set; }

        [Required]
        [Range(0, 99)]
        public int CardExpiryYear { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string? Currency { get; set; }
    }
}
