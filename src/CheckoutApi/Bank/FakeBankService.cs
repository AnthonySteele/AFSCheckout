using System.Threading.Tasks;
using CheckoutApi.Controllers;

namespace CheckoutApi.Bank
{
    public class FakeBankService : IBankService
    {
        public Task<BankResponse> ProcessPayment(PaymentRequest request)
        {
            var result = new BankResponse();
            return Task.FromResult(result);
        }
    }
}
