using System.Data.Common;

namespace CompanyHierarchy.Infrastructure.Data;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}