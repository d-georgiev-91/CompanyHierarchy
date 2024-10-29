using CompanyHierarchy.Domain.Entities;
using MediatR;

namespace CompanyHierarchy.Domain.Queries;

public class GetAllEmployeesWithManagedEmployeesQuery : IRequest<IEnumerable<Employee>>
{
}
