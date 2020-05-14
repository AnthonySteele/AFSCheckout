using Prometheus;

namespace CheckoutApi
{
    public static class CustomMetrics
    {
        public static readonly Counter PaymentStarted = Metrics.CreateCounter("payment_started", "Payment request started");
        public static readonly Counter PaymentCompleted = Metrics.CreateCounter("payment_completed", "Payment request completed", new CounterConfiguration
        {
            LabelNames = new[] { "success" }
        });
    }
}
