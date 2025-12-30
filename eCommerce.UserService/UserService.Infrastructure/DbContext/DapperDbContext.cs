using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace eCommerce.UserService.Infrastructure.DbContext;

public class DapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _connection;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        string connectionString = _configuration.GetConnectionString("PostgresConnection");

        // replavce connection string with environment variable if exists
        connectionString = connectionString
                            .Replace("$POSTGRESDB_HOST", Environment.GetEnvironmentVariable("POSTGRESDB_HOST") ?? "localhost")
                            .Replace("$POSTGRESDB_PORT", Environment.GetEnvironmentVariable("POSTGRESDB_PORT") ?? "5433")
                            .Replace("$POSTGRESDB_NAME", Environment.GetEnvironmentVariable("POSTGRESDB_NAME") ?? "eCommerceUsers")
                            .Replace("$POSTGRESDB_USER", Environment.GetEnvironmentVariable("POSTGRESDB_USER") ?? "postgres")
                            .Replace("$POSTGRESDB_PASSWORD", Environment.GetEnvironmentVariable("POSTGRESDB_PASSWORD") ?? "postgres");

        //Console.WriteLine($"Using connection string: {connectionString}");

        //Create a new NpgsqlConnection with the retrieved connection string
        _connection = new NpgsqlConnection(connectionString);
    }

    public IDbConnection DbConnection => _connection;
}