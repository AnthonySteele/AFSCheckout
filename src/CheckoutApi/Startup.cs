using CheckoutApi.Bank;
using CheckoutApi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Prometheus;

namespace CheckoutApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAquiringBankService, FakeAquiringBankService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddSingleton<IPaymentRepository, FakePaymentRepository>();

            services.AddOpenApiDocument(config =>
            {
                config.Title = "Payment API";
                config.Description = "AFS payment api demo";
            });

            services
                .AddMvc()
                .AddNewtonsoftJson(opts => opts.SerializerSettings.Converters.Add(new StringEnumConverter()))
                .AddControllersAsServices();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseMetricServer();
        }
    }
}
