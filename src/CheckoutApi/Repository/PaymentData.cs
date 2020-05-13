using System;

namespace CheckoutApi.Repository
{
    public class PaymentData
    {
        public Guid TransactionId { get; set; }
        public string CardNumber { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
