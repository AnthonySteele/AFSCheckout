using CheckoutApi.Controllers;

namespace CheckoutApi.IntegrationTests
{
    public static class PaymentData
    {
        public static PaymentRequest ValidPaymentRequest()
        {
            return new PaymentRequest
            {
                NameOnCard = "Mr A Test",
                CardNumber = "1234123412341234",
                CardCvv = "123",
                CardExpiryMonth = 04,
                CardExpiryYear = 24,
                Currency = "GBP",
                Amount = 100m
            };
        }
    }
}
