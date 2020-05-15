using System;
using System.Net;
using System.Threading.Tasks;
using CheckoutApi.Controllers;
using CheckoutApi.IntegrationTests.Infrastructure;
using CheckoutApi.Repository;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CheckoutApi.IntegrationTests
{
    public class GetPaymentTests
    {
        [Test]
        public async Task UnknownIdIsNotFound()
        {
            var unknownId = Guid.NewGuid();
            var response = await TestFixture.Client.GetAsync($"/payment/{unknownId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task BadIdIsNotFound()
        {
            var response = await TestFixture.Client.GetAsync($"/payment/notaguid");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task KnownIdIsFound()
        {
            var knownId = await GetValidPaymentId();
            var response = await TestFixture.Client.GetAsync($"/payment/{knownId}");

            await HttpAssert.IsOkWithJsonContent(response);
        }

        [Test]
        public async Task StoredDataIsCorrect()
        {
            var paymentRequest = PaymentRequestBuilder.ValidPaymentRequest();
            var paymentId = await SubmitPayment(paymentRequest);

            var getResponse = await TestFixture.Client.GetAsync($"/payment/{paymentId}");

            var responseContent = await getResponse.Content.ReadAsStringAsync();
            var bankResponse = JsonConvert.DeserializeObject<PaymentData>(responseContent);


            Assert.That(bankResponse.Id, Is.EqualTo(paymentId));
            Assert.That(bankResponse.Amount, Is.EqualTo(paymentRequest.Amount));
            Assert.That(bankResponse.NameOnCard, Is.EqualTo(paymentRequest.NameOnCard));
            Assert.That(bankResponse.CardNumber, Is.EqualTo("4111"));
        }

        private static async Task<Guid> GetValidPaymentId()
        {
            var payment = PaymentRequestBuilder.ValidPaymentRequest();
            return await SubmitPayment(payment);
        }

        private static async Task<Guid> SubmitPayment(PaymentRequest payment)
        { 
            var response = await TestFixture.Client.PostAsync("/payment", ContentHelpers.JsonString(payment));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseContent = await response.Content.ReadAsStringAsync();

            var paymentResponse = JsonConvert.DeserializeObject<PaymentData>(responseContent);
            Assert.That(paymentResponse, Is.Not.Null);
            return paymentResponse.Id;
        }
    }
}
