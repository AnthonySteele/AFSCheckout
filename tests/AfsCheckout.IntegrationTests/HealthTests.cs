using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CheckoutApi.IntegrationTests
{
    public class HealthTests
    {
        [Test]
        public async Task HealthCheckReturnsOk()
        {
            var response = await TestFixture.Client.GetAsync("/health");

            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task RandomPathIsNotFound()
        {
            var response = await TestFixture.Client.GetAsync("/somepath/thatisnot/used");

            Assert.That(response.IsSuccessStatusCode, Is.False);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
