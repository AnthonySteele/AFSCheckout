using System;
using System.Threading.Tasks;
using CheckoutApi.Controllers;

namespace CheckoutApi.Bank
{
    public class FakeBankService : IBankService
    {
        public Task<BankResponse> ProcessPayment(PaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var success = !request.NameOnCard.Contains("fail", StringComparison.OrdinalIgnoreCase);

            var result = new BankResponse
            {
                Success = success,
                TransactionId = Guid.NewGuid()
            };

            return Task.FromResult(result);
        }
    }
}
