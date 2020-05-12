using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CheckoutApi.IntegrationTests.Infrastructure
{
    public class TestServerFixture : IDisposable
    {
        public TestServer TestServer { get; private set; }
        public HttpClient Client { get; private set; }
        public IConfiguration Configuration { get; }

        public TestServerFixture()
        {
            var configFixture = new ConfigurationFixture();
            Configuration = configFixture.Configuration;

            TestServer = BuildTestServer();
            Client = CreateClient();
        }

        private HttpClient CreateClient()
        {
            return TestServer.CreateClient();
        }

        private TestServer BuildTestServer()
        {
            var builder = TestServerBuilder<Startup>(Configuration);
            return new TestServer(builder);
        }

        private static IWebHostBuilder TestServerBuilder<T>(IConfiguration config) where T : class
        {
            return new WebHostBuilder()
                .UseConfiguration(config)
                .UseStartup<T>()
                .ConfigureLogging(logging => logging.AddConsole());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Client?.Dispose();
                TestServer?.Dispose();
            }
        }
    }
}
