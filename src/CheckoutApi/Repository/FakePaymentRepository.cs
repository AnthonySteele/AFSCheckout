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

        public void Add(PaymentData request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _data[request.Id] = request;
        }

        public void Update(Guid id, Guid bankTransactionId, PaymentStatus status)
        {
            var item = GetPaymentById(id);
            if (item == null)
            {
                throw new ArgumentException("Cannot find item to update", nameof(id));
            }

            item.BankTransactionId = bankTransactionId;
            item.Status = status;
        }

        public IEnumerable<PaymentData> AllData()
        {
            return _data.Values;
        }
    }
}
