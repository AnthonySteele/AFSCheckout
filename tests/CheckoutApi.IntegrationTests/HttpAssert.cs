using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CheckoutApi.IntegrationTests
{
    public static class HttpAssert
    {
        public static async Task IsOkWithJsonContent(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.That(responseContent, Is.Not.Empty);

            var jsonData = JObject.Parse(responseContent);
            Assert.That(jsonData, Is.Not.Null);
        }

        public static async Task IsBadRequestWithJsonContent(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(responseContent, Is.Not.Empty);
            Assert.That(responseContent, Does.Contain("One or more validation errors occurred."));

            var jsonData = JObject.Parse(responseContent);
            Assert.That(jsonData, Is.Not.Null);
        }
    }
}
