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
            string connectionString = _config.GetConnectionString("PostgresConnection")!;

            // Create new Npgsql Connection
            _dbConnection = new NpgsqlConnection(connectionString);
        }

        public IDbConnection DbConnection => _dbConnection;
    }
}
