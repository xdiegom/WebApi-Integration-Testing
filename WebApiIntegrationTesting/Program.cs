using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebApiIntegrationTesting.DataAccess.Context;
using WebApiIntegrationTesting.DataAccess.Repositories;
using WebApiIntegrationTesting.Middlewares;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<HandleRequestValidationFilter>();
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Optional
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ReviewContext>(options =>
        {
            var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            string connectionString = configuration.GetConnectionString("DefaultConnectionString")!;
            options.UseSqlServer(connectionString);
        }
        );

        builder.Services.AddScoped<IReviewRepository, SQLiteRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.MapControllers();

        app.Run();
    }
}