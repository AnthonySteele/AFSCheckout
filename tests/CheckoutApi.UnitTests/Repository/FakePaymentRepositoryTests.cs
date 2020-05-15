using System;
using CheckoutApi.Repository;
using NUnit.Framework;

namespace CheckoutApi.UnitTests.Repository
{
    public class FakePaymentRepositoryTests
    {
        [Test]
        public void EmptyRepoReturnsNulls()
        {
            var repo = new FakePaymentRepository();

            var item = repo.GetPaymentById(Guid.NewGuid());

            Assert.That(item, Is.Null);
        }

        [Test]
        public void CanSaveAndRetrieve()
        {
            var repo = new FakePaymentRepository();
            var newItem = BuildPaymentData();

            repo.Add(newItem);
            var retrievedItem = repo.GetPaymentById(newItem.Id);

            AssertDataEqual(retrievedItem, newItem);
            Assert.That(retrievedItem!.BankTransactionId, Is.Null);
        }

        [Test]
        public void CanUpdate()
        {
            var repo = new FakePaymentRepository();
            var newItem = BuildPaymentData();
            var bankTransactionId = Guid.NewGuid();

            repo.Add(newItem);
            repo.Update(newItem.Id, bankTransactionId, PaymentStatus.Accepted);

            var retrievedItem = repo.GetPaymentById(newItem.Id);

            Assert.That(retrievedItem!.Id, Is.EqualTo(newItem.Id));
            Assert.That(retrievedItem.Status, Is.EqualTo(PaymentStatus.Accepted));
            Assert.That(retrievedItem.BankTransactionId, Is.EqualTo(bankTransactionId));
        }

        private static void AssertDataEqual(PaymentData? actual, PaymentData expected)
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual!.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Amount, Is.EqualTo(expected.Amount));
            Assert.That(actual.CardNumber, Is.EqualTo(expected.CardNumber));
            Assert.That(actual.NameOnCard, Is.EqualTo(expected.NameOnCard));
            Assert.That(actual.Status, Is.EqualTo(expected.Status));
            Assert.That(actual.BankTransactionId, Is.EqualTo(expected.BankTransactionId));
        }

        private static PaymentData BuildPaymentData()
        {
            return new PaymentData
            {
                Id = Guid.NewGuid(),
                Amount = 100.0m,
                CardNumber = "4111",
                Status = PaymentStatus.Received,
                NameOnCard = "A Test item"
            };
        }
    }
}
