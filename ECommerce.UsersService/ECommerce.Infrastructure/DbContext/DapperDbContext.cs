using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.DbContext
{
    public class DapperDbContext
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _dbConnection;
        public DapperDbContext(IConfiguration config)
        {
            _config = config;
            string connectionStringTemplate = _config.GetConnectionString("PostgresConnection")!;
            string connectionString = connectionStringTemplate
                .Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
                .Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"))
                .Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"))
                .Replace("$POSTGRES_USER", Environment.GetEnvironmentVariable("POSTGRES_USER"))
                .Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT"));
            // Create new Npgsql Connection
            _dbConnection = new NpgsqlConnection(connectionString);
        }

        public IDbConnection DbConnection => _dbConnection;
    }
}
