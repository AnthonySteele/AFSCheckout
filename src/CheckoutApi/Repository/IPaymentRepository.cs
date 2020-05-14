using System;

namespace CheckoutApi.Repository
{
    public interface IPaymentRepository
    {
        void Save(PaymentData request);
        PaymentData? GetPaymentById(Guid id);
    }
}
