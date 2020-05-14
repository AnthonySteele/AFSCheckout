using System.Threading.Tasks;
using CheckoutApi.Controllers;

namespace CheckoutApi.Bank
{
    public interface IAquiringBankService
    {
        Task<BankResponse> ProcessPayment(PaymentRequest request);
    }
}
