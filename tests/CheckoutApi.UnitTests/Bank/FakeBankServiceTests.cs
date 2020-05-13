using System.Threading.Tasks;
using CheckoutApi.Bank;
using CheckoutApi.Controllers;
using NUnit.Framework;

namespace CheckoutApi.UnitTests.Bank
{
    public class FakeBankServiceTests
    {
        [Test]
        public async Task CanProcessPayment()
        {
            var bankService = new FakeBankService();
            var payment = new PaymentRequest();

            var result = await bankService.ProcessPayment(payment);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
        }
    }
}
