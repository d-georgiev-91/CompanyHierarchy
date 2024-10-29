using CompanyHierarchy.Domain.Entities;
using MediatR;

namespace CompanyHierarchy.Domain.Queries;

public class GetEmployeeWithManagedEmployeesQuery : IRequest<Employee?>
{
    public int EmployeeId { get; set; }
}