using CompanyHierarchy.Domain.Entities;
using CompanyHierarchy.Domain.Queries;
using CompanyHierarchy.Domain.Repositories;
using MediatR;

namespace CompanyHierarchy.Infrastructure.Handlers.QueryHandlers;

public class GetEmployeeWithManagedEmployeesQueryHandler(IEmployeeRepository employeeRepository)
    : IRequestHandler<GetEmployeeWithManagedEmployeesQuery, Employee?>
{
    public Task<Employee?> Handle(GetEmployeeWithManagedEmployeesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(employeeRepository.GetByIdWithManagedEmployees(request.EmployeeId));
    }
}