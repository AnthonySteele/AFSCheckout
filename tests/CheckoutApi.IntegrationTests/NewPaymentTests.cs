using System.Net;
using System.Threading.Tasks;
using CheckoutApi.IntegrationTests.Infrastructure;
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

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(responseContent, Is.Not.Empty);
            Assert.That(responseContent, Does.Contain("{\"success\":true,"));
        }

        [Test]
        public async Task BankRejection()
        {
            var payment = PaymentRequestBuilder.ValidPaymentRequest();
            payment.NameOnCard = "Mr A fail";

            var response = await TestFixture.Client.PostAsync("/payment", ContentHelpers.JsonString(payment));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(responseContent, Is.Not.Empty);
            Assert.That(responseContent, Does.Contain("{\"success\":false,"));
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
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task PutIsNotAccepted()
        {
            var payment = PaymentRequestBuilder.ValidPaymentRequest();

            var response = await TestFixture.Client.PutAsync("/payment", ContentHelpers.JsonString(payment));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }
    }
}
