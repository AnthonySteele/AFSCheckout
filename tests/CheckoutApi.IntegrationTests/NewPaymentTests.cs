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

            await HttpAssert.IsBadRequestWithJsonContent(response);
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

        [Test]
        public async Task BankRejection()
        {
            var payment = PaymentData.ValidPaymentRequest();
            payment.NameOnCard = "Mr A fail";

            var response = await TestFixture.Client.PutAsync("/payment", ContentHelpers.JsonString(payment));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(responseContent, Is.Not.Empty);
            Assert.That(responseContent, Does.Contain("{\"success\":false,"));
        }

        [Test]
        public async Task PaymentDataWithoutNameIsNotAccepted()
        {
            var payment = PaymentData.ValidPaymentRequest();
            payment.NameOnCard = string.Empty;

            var response = await TestFixture.Client.PutAsync("/payment", ContentHelpers.JsonString(payment));

            await HttpAssert.IsBadRequestWithJsonContent(response);
        }

        [Test]
        public async Task PaymentDataWithInvalidCreditCardNumberIsNotAccepted()
        {
            var payment = PaymentData.ValidPaymentRequest();
            payment.CardNumber = "nosuch";

            var response = await TestFixture.Client.PutAsync("/payment", ContentHelpers.JsonString(payment));

            await HttpAssert.IsBadRequestWithJsonContent(response);
        }
    }
}
