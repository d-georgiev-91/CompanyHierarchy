using System.Data;
using Npgsql;

namespace CompanyHierarchy.Infrastructure.Data;

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(connectionString);
    }
}