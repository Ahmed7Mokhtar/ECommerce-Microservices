using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OrdersService.BusinessLogicLayer.Repositories;
using OrdersService.DataAccessLayer.RepositoryContracts;

namespace OrdersService.DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration config)
        {
            string connectionStringTemplate = config.GetConnectionString("MongoDB")!;
            var connectionString = connectionStringTemplate
                .Replace("$MONGODB_HOST", Environment.GetEnvironmentVariable("MONGODB_HOST"))
                .Replace("$MONGODB_PORT", Environment.GetEnvironmentVariable("MONGODB_PORT"));

            services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

            services.AddScoped<IMongoDatabase>(provider =>
            {
                IMongoClient client = provider.GetRequiredService<IMongoClient>();
                return client.GetDatabase("OrdersDatabase");
            });

            services.AddScoped<IOrdersRepository, OrdersRepository>();

            return services;
        }
    }
}
