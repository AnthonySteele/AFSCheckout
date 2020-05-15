using System;
using System.Linq;
using System.Threading.Tasks;
using CheckoutApi.Bank;
using CheckoutApi.Controllers;
using CheckoutApi.Repository;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
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

            var response = await service.ProcessPayment(PaymentRequestBuilder.ValidPaymentRequest());

            Assert.That(response.Status, Is.EqualTo(PaymentStatus.Accepted));
            Assert.That(response.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(response.BankTransactionId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task PaymentCanBeRejected()
        {
            var service = BuildPaymentService();
            var request = PaymentRequestBuilder.ValidPaymentRequest();
            request.NameOnCard = "Mr A Fail";

            var response = await service.ProcessPayment(request);

            Assert.That(response.Status, Is.EqualTo(PaymentStatus.Rejected));
            Assert.That(response.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(response.BankTransactionId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task KnownPaymentIdReturnsData()
        {
            var service = BuildPaymentService();

            var response = await service.ProcessPayment(PaymentRequestBuilder.ValidPaymentRequest());

            var payment = service.GetPaymentById(response.Id);

            Assert.That(payment, Is.Not.Null);
            Assert.That(payment!.Status, Is.EqualTo(PaymentStatus.Accepted));
        }

        [Test]
        public async Task RejectedPaymentReturnsDataFromrepository()
        {
            var service = BuildPaymentService();

            var request = PaymentRequestBuilder.ValidPaymentRequest();
            request.NameOnCard = "Mr A Fail";
            var response = await service.ProcessPayment(request);

            var payment = service.GetPaymentById(response.Id);

            Assert.That(payment, Is.Not.Null);
            Assert.That(payment!.Status, Is.EqualTo(PaymentStatus.Rejected));
        }

        [Test]
        public void FailingBankHasDataInRepository()
        {
            var failingBank = new Mock<IAquiringBankService>();
            failingBank.Setup(r => r.ProcessPayment(It.IsAny<PaymentRequest>()))
                .ThrowsAsync(new InvalidOperationException("fail"));

            var paymentsRepo = new FakePaymentRepository();

            var service = new PaymentService(failingBank.Object,
                paymentsRepo,
                NullLogger<PaymentService>.Instance);

            var request = PaymentRequestBuilder.ValidPaymentRequest();

            Assert.ThrowsAsync<InvalidOperationException>(() => service.ProcessPayment(request));

            var savedItem = paymentsRepo.AllData().FirstOrDefault();

            Assert.That(savedItem, Is.Not.Null);
            Assert.That(savedItem.Status, Is.EqualTo(PaymentStatus.Received));
            Assert.That(savedItem.BankTransactionId, Is.Null);
        }

        private static IPaymentService BuildPaymentService()
        {
            return new PaymentService(
                new FakeAquiringBankService(), new FakePaymentRepository(),
                NullLogger<PaymentService>.Instance);
        }
    }
}
