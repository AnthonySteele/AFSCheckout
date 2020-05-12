using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CheckoutApi.IntegrationTests
{
    public class HealthTest
    {
        [Test]
        public async Task HealthCheckReturnsOk()
        {
            var response = await TestFixture.Client.GetAsync("/health");

            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
