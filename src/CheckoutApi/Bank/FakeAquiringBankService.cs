using System;
using System.Threading.Tasks;
using CheckoutApi.Controllers;

namespace CheckoutApi.Bank
{
    public class FakeAquiringBankService : IAquiringBankService
    {
        public Task<BankResponse> ProcessPayment(PaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new BankResponse
            {
                Success = FakePaymentSuccess(request),
                TransactionId = Guid.NewGuid()
            };

            return Task.FromResult(result);
        }

        private static bool FakePaymentSuccess(PaymentRequest request)
        {
            return !request.NameOnCard.Contains("fail", StringComparison.OrdinalIgnoreCase);
        }
    }
}
