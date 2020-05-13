using System;
using System.Net.Http;
using CheckoutApi.IntegrationTests.Infrastructure;
using NUnit.Framework;

namespace CheckoutApi.IntegrationTests
{
    [SetUpFixture]
    public sealed class TestFixture: IDisposable
    {
        private static TestServerFixture? _server;

        public static TestServerFixture Server => _server ?? throw new InvalidOperationException("Server has been disposed");

        public static HttpClient Client => Server.Client;

        [OneTimeSetUp]
        public void SetUp()
        {
            _server = new TestServerFixture();
        }

        public void Dispose()
        {
            _server?.Dispose();
            _server = null;
        }
    }
}
