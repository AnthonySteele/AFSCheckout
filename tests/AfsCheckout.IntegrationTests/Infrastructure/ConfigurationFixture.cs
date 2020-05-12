using Microsoft.Extensions.Configuration;

namespace CheckoutApi.IntegrationTests.Infrastructure
{
    public class ConfigurationFixture
    {
        public ConfigurationFixture()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.test.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
    }
}
