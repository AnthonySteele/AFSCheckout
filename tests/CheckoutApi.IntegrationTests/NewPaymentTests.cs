using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CheckoutApi.IntegrationTests.Infrastructure;
using CheckoutApi.Repository;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CheckoutApi.IntegrationTests
{
    public class NewPaymentTests
    {
        [Test]
        public async Task NoPaymentDataIsBadRequest()
        {
            var response = await TestFixture.Client.PostAsync("/payment", ContentHelpers.JsonString("{}"));

            await HttpAssert.IsBadRequestWithJsonContent(response);
        }

        [Test]
        public async Task ValidPaymentDataIsAccepted()
        {
            var payment = PaymentRequestBuilder.ValidPaymentRequest();

            var response = await TestFixture.Client.PostAsync("/payment", ContentHelpers.JsonString(payment));

            var paymentResponse = await ReadAsPaymentData(response);
            Assert.That(paymentResponse.Status, Is.EqualTo(PaymentStatus.Accepted));
        }

        [Test]
        public async Task BankRejectionHasCorrectStatus()
        {
            var payment = PaymentRequestBuilder.RequestToBeRejected();

            var response = await TestFixture.Client.PostAsync("/payment", ContentHelpers.JsonString(payment));

            var paymentResponse = await ReadAsPaymentData(response);
            Assert.That(paymentResponse.Status, Is.EqualTo(PaymentStatus.Rejected));
        }

        [Test]
        public async Task PaymentDataWithoutNameIsNotAccepted()
        {
            var payment = PaymentRequestBuilder.ValidPaymentRequest();
            payment.NameOnCard = string.Empty;

            var response = await TestFixture.Client.PostAsync("/payment", ContentHelpers.JsonString(payment));

            await HttpAssert.IsBadRequestWithJsonContent(response);
        }

        [Test]
        public async Task PaymentDataWithInvalidCreditCardNumberIsNotAccepted()
        {
            var payment = PaymentRequestBuilder.ValidPaymentRequest();
            payment.CardNumber = "nosuch";

            var response = await TestFixture.Client.PostAsync("/payment", ContentHelpers.JsonString(payment));

            await HttpAssert.IsBadRequestWithJsonContent(response);
        }

        [Test]
        public async Task AmexCardNumberIsAccepted()
        {
            var payment = PaymentRequestBuilder.ValidPaymentRequest();
            payment.CardNumber = "378282246310005";
            payment.CardCvv = "1234";

            var response = await TestFixture.Client.PostAsync("/payment", ContentHelpers.JsonString(payment));

            var paymentResponse = await ReadAsPaymentData(response);
            Assert.That(paymentResponse.Status, Is.EqualTo(PaymentStatus.Accepted));
        }

        [Test]
        public async Task PutVerbIsNotAccepted()
        {
            var payment = PaymentRequestBuilder.ValidPaymentRequest();

            var response = await TestFixture.Client.PutAsync("/payment", ContentHelpers.JsonString(payment));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        private static async Task<PaymentData> ReadAsPaymentData(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(responseContent, Is.Not.Empty);
            var paymentResponse = JsonConvert.DeserializeObject<PaymentData>(responseContent);
            Assert.That(paymentResponse, Is.Not.Null);
            Assert.That(paymentResponse.Id, Is.Not.EqualTo(Guid.Empty));
            return paymentResponse;
        }
    }
}
