using System.Net;
using System.Threading.Tasks;
using CheckoutApi.Controllers;
using CheckoutApi.IntegrationTests.Infrastructure;
using NUnit.Framework;

namespace CheckoutApi.IntegrationTests
{
    public class NewPaymentTests
    {
        [Test]
        public async Task NoPaymentDataIsBadRequest()
        {
            var response = await TestFixture.Client.PutAsync("/payment", ContentHelpers.JsonString("{}"));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(responseContent, Is.Not.Empty);
            Assert.That(responseContent, Does.Contain("One or more validation errors occurred."));
        }

        [Test]
        public async Task ValidPaymentDataIsAccepted()
        {
            var payment = PaymentData.ValidPaymentRequest();

            var response = await TestFixture.Client.PutAsync("/payment", ContentHelpers.JsonString(payment));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(responseContent, Is.Not.Empty);
            Assert.That(responseContent, Does.Contain("{\"success\":true,"));
        }
    }
}
