using System;

namespace CheckoutApi.Repository
{
    public interface IPaymentRepository
    {
        PaymentData? GetPaymentById(Guid id);
        void Add(PaymentData request);
        void Update(Guid id, Guid bankTransactionId, PaymentStatus status);
    }
}
