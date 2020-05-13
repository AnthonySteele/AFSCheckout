using System;
using System.Net;
using System.Threading.Tasks;
using CheckoutApi.Controllers;
using CheckoutApi.IntegrationTests.Infrastructure;
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

        private static async Task<Guid> GetValidPaymentId()
        {
            var payment = PaymentData.ValidPaymentRequest();

            var response = await TestFixture.Client.PutAsync("/payment", ContentHelpers.JsonString(payment));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseContent = await response.Content.ReadAsStringAsync();

            var bankResponse = JsonConvert.DeserializeObject<BankResponse>(responseContent);
            Assert.That(bankResponse, Is.Not.Null);
            return bankResponse.TransactionId;
        }
    }
}
