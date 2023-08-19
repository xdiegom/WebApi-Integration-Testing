using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApiIntegrationTesting.DataAccess.Context;
using Xunit;

namespace WebApiIntegrationTesting.IntegrationTests.ControllerTests
{
    public class BaseTest : IClassFixture<CustomWebApplicationFactory>, IDisposable
    {
        protected CustomWebApplicationFactory _factory;
        protected HttpClient _client;
        protected readonly ReviewContext _dbContext;

        public BaseTest()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();

            var scope = _factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ReviewContext>();

            // Aplicar migraciones
            _dbContext.Database.Migrate();
        }


        public void Dispose()
        {
            // Here we are deleting database tables to ensure data used in each test
            // is not mixed.
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ReviewContext>();

            /**
             * Uncomment if database has to be deleted
             * */
            dbContext.Database.EnsureDeleted();

            /**
             * OR: Uncomment if database tables are truncated instead of deleting database
             * */
            //// Obtener el nombre de todas las tablas
            //var tableNames = dbContext.Model.GetEntityTypes().Select(e => e.GetTableName());

            //// Truncate de todas las tablas (eliminar los datos sin eliminar la estructura)
            //foreach (var tableName in tableNames)
            //{
            //    dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE {tableName}");
            //}

            //// Si es necesario, puedes reiniciar secuencias de identidad
            //foreach (var tableName in tableNames)
            //{
            //    dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT('{tableName}', RESEED, 1)");
            //}

            _client.Dispose();
            _factory.Dispose();
        }
    }
}