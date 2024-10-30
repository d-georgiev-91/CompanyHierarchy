using CompanyHierarchy.Infrastructure.Handlers.QueryHandlers.Employee;

namespace CompanyHierarchy.Domain.Queries.Employee;

public class GetAllEmployeesWithManagedEmployeesQuery : IQuery<IEnumerable<EmployeeResponse>>
{
}
