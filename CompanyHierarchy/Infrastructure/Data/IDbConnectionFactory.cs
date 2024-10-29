using System.Data;

namespace CompanyHierarchy.Infrastructure.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}