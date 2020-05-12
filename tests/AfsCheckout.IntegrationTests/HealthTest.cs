using System.Net;
using System.Threading.Tasks;
using CheckoutApi.IntegrationTests.Infrastructure;
using NUnit.Framework;

namespace CheckoutApi.IntegrationTests
{
    public class HealthTest
    {
        [Test]
        public async Task HealthCheckReturnsOk()
        {
            using var server = new TestServerFixture();

            var response = await server.Client.GetAsync("/health");

            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
