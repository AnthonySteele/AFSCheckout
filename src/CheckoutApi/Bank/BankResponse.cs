using System;

namespace CheckoutApi.Controllers
{
    public class BankResponse
    {
        public bool Success { get; set; }
        public Guid TransactionId { get; set; }
    }
}
