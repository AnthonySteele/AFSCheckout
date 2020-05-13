using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace CheckoutApi.IntegrationTests.Infrastructure
{
    public static class ContentHelpers
    {
        public const string JsonMediaType = @"application/json";

        public static StringContent EmptyString()
        {
            return new StringContent(string.Empty);
        }

        public static StringContent JsonString(string data)
        {
            return new StringContent(data, Encoding.UTF8, JsonMediaType);
        }

        public static StringContent JsonString<T>(T data)
        {
            return JsonString(JsonConvert.SerializeObject(data));
        }
    }
}
