using System.Data.Common;
using Npgsql;

namespace CompanyHierarchy.Infrastructure.Data;

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public DbConnection CreateConnection() => new NpgsqlConnection(connectionString);
}