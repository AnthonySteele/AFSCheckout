using System;
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
            Assert.That(result.TransactionId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task TwoPaymentsHaveDifferentIds()
        {
            var bankService = new FakeBankService();
            var payment1 = new PaymentRequest();
            var payment2 = new PaymentRequest();

            var result1 = await bankService.ProcessPayment(payment1);
            var result2 = await bankService.ProcessPayment(payment2);

            Assert.That(result1.TransactionId, Is.Not.EqualTo(result2.TransactionId));
        }
    }
}
