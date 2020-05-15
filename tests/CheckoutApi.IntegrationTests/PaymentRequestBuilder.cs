using CheckoutApi.Controllers;

namespace CheckoutApi.IntegrationTests
{
    public static class PaymentRequestBuilder
    {
        public static PaymentRequest ValidPaymentRequest()
        {
            return new PaymentRequest
            {
                NameOnCard = "Mr A Test",
                CardNumber = "4111111111111111",
                CardCvv = "123",
                CardExpiryMonth = 04,
                CardExpiryYear = 24,
                Currency = "GBP",
                Amount = 100.0m
            };
        }

        public static PaymentRequest RequestToBeRejected()
        {
            var request = ValidPaymentRequest();
            request.NameOnCard = "Mr A Fail";
            return request;
        }
    }
}
