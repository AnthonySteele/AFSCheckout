using System.Threading.Tasks;
using CheckoutApi.Controllers;

namespace CheckoutApi.Bank
{
    public interface IBankService
    {
        Task<BankResponse> ProcessPayment(PaymentRequest request);
    }
}
