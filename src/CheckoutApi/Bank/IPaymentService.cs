using System;
using System.Threading.Tasks;
using CheckoutApi.Controllers;
using CheckoutApi.Repository;

namespace CheckoutApi.Bank
{
    public interface IPaymentService
    {
        Task<PaymentData> ProcessPayment(PaymentRequest request);
        PaymentData? GetPaymentById(Guid guidId);
    }
}
