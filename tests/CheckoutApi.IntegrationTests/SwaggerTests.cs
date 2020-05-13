using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CheckoutApi.IntegrationTests
{
    public class SwaggerTests
    {
        [Test]
        public async Task CanLoadSwagger()
        {
            var response = await TestFixture.Client.GetAsync("/swagger");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
        }

        [Test]
        public async Task CanLoadSwaggerAtDirectUrl()
        {
            var response = await TestFixture.Client.GetAsync("/swagger/index.html");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task CanLoadSwaggerData()
        {
            var response = await TestFixture.Client.GetAsync("/swagger/v1/swagger.json");
            await HttpAssert.IsOkWithJsonContent(response);
        }
    }
}
