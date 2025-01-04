using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.DataAccessLayer.Context;
using ProductsService.DataAccessLayer.Repositories;
using ProductsService.DataAccessLayer.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsService.DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration config)
        {
            string connectionStringTemplate = config.GetConnectionString("MySqlConnection")!;
            var connectionString = connectionStringTemplate
                .Replace("$MYSQL_HOST", Environment.GetEnvironmentVariable("MYSQL_HOST"))
                .Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD"));
            services.AddDbContext<AppDbContext>(opts =>
            {
                opts.UseMySQL(connectionString);
            });

            services.AddScoped<IProductsRepository, ProductsRepository>();

            return services;
        }
    }
}
