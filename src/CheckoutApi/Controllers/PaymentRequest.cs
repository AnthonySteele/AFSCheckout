using System.ComponentModel.DataAnnotations;

namespace CheckoutApi.Controllers
{
    public class PaymentRequest
    {
        [Required]
        [StringLength(16, MinimumLength = 16)]
        public string? CardNumber { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string? CardCvv { get; set; }

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
