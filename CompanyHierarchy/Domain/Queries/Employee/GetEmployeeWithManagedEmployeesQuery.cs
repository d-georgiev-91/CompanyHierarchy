using CompanyHierarchy.Infrastructure.Handlers.QueryHandlers.Employee;

namespace CompanyHierarchy.Domain.Queries.Employee;

public class GetEmployeeWithManagedEmployeesQuery : IQuery<EmployeeResponse>
{
    public int EmployeeId { get; set; }
}