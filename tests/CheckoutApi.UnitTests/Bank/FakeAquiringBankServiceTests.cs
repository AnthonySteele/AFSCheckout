using System;
using System.Threading.Tasks;
using CheckoutApi.Bank;
using NUnit.Framework;

namespace CheckoutApi.UnitTests.Bank
{
    public class FakeAquiringBankServiceTests
    {
        [Test]
        public async Task CanProcessPayment()
        {
            var bankService = new FakeAquiringBankService();
            var payment = PaymentRequestBuilder.ValidPaymentRequest();

            var result = await bankService.ProcessPayment(payment);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.TransactionId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task PaymentCanFail()
        {
            var bankService = new FakeAquiringBankService();
            var payment = PaymentRequestBuilder.ValidPaymentRequest();
            payment.NameOnCard = "Test A. Fail";

            var result = await bankService.ProcessPayment(payment);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.TransactionId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task TwoPaymentsHaveDifferentIds()
        {
            var bankService = new FakeAquiringBankService();
            var payment1 = PaymentRequestBuilder.ValidPaymentRequest();
            var payment2 = PaymentRequestBuilder.ValidPaymentRequest();
            payment2.Amount = 120.0m;
            payment2.NameOnCard = "A.N. Other";

            var result1 = await bankService.ProcessPayment(payment1);
            var result2 = await bankService.ProcessPayment(payment2);

            Assert.That(result1.TransactionId, Is.Not.EqualTo(result2.TransactionId));
        }
    }
}
