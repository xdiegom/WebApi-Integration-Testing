using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApiIntegrationTesting.DataAccess.Context;
using WebApiIntegrationTesting.DataAccess.Repositories;
using Xunit;

namespace WebApiIntegrationTesting.IntegrationTests.ControllerTests
{
    public class CustomWebApplicationFactory: WebApplicationFactory<Program>
    {
        public Mock<IReviewRepository> ReviewRepositoryMock { get; }

        public CustomWebApplicationFactory()
        {
            ReviewRepositoryMock = new Mock<IReviewRepository>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true);
            });

            builder.ConfigureTestServices(services =>
            {
                var webHostBuilder = builder.ConfigureServices(services =>
                {
                    var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                    string connectionString = configuration.GetConnectionString("DefaultConnectionString")!;

                    services.AddDbContext<ReviewContext>(options =>
                    {
                        options.UseSqlServer(connectionString);
                    });
                });
            });
        }
    }
}
