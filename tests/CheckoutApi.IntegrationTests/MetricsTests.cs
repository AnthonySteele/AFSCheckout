using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CheckoutApi.IntegrationTests
{
    public class MetricsTests
    {
        [Test]
        public async Task CanLoadMetrics()
        {
            var response = await TestFixture.Client.GetAsync("/metrics");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
