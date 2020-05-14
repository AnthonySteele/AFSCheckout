using System;
using System.Collections.Generic;

namespace CheckoutApi.Repository
{
    public class FakePaymentRepository : IPaymentRepository
    {
        private readonly IDictionary<Guid, PaymentData> _data = new Dictionary<Guid, PaymentData>();

        public PaymentData? GetPaymentById(Guid id)
        {
            if (_data.TryGetValue(id, out var item))
            {
                return item;
            }

            return null;
        }

        public void Save(PaymentData request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _data[request.TransactionId] = request;
        }
    }
}
