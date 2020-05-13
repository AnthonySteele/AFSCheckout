using System;

namespace CheckoutApi.Repository
{
    public interface IPaymentRepository
    {
        void Save(PaymentData request);
        PaymentData? Read(Guid id);
    }
}
