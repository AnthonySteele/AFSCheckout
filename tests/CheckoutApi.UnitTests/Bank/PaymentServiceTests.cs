using System;
using System.Threading.Tasks;
using CheckoutApi.Bank;
using CheckoutApi.Repository;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace CheckoutApi.UnitTests.Bank
{
    public class PaymentServiceTests
    {
        [Test]
        public void UnknownPaymentIdReturnsNull()
        {
            var service = BuildPaymentService();

            var payment = service.GetPaymentById(Guid.NewGuid());

            Assert.That(payment, Is.Null);
        }

        [Test]
        public async Task ValidPaymentIsProcessed()
        {
            var service = BuildPaymentService();

            var response = await service.ProcessPayment(PaymentData.ValidPaymentRequest());

            Assert.That(response.Success, Is.True);
            Assert.That(response.TransactionId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task PaymentCanFail()
        {
            var service = BuildPaymentService();
            var request = PaymentData.ValidPaymentRequest();
            request.NameOnCard = "Mr A Fail";

            var response = await service.ProcessPayment(request);

            Assert.That(response.Success, Is.False);
            Assert.That(response.TransactionId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task KnownPaymentIdReturnsData()
        {
            var service = BuildPaymentService();

            var response = await service.ProcessPayment(PaymentData.ValidPaymentRequest());

            var payment = service.GetPaymentById(response.TransactionId);

            Assert.That(payment, Is.Not.Null);
        }

        private static IPaymentService BuildPaymentService()
        {
            return new PaymentService(
                new FakeBankService(), new FakePaymentRepository(),
                NullLogger<PaymentService>.Instance);
        }
    }
}
