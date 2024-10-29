using CompanyHierarchy.Domain.Entities;
using CompanyHierarchy.Domain.Queries;
using CompanyHierarchy.Domain.Repositories;
using MediatR;

namespace CompanyHierarchy.Infrastructure.Handlers.QueryHandlers;

public class GetAllEmployeesWithManagedEmployeesQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetAllEmployeesWithManagedEmployeesQuery, IEnumerable<Employee>>
{
    public Task<IEnumerable<Employee>> Handle(GetAllEmployeesWithManagedEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = employeeRepository.GetAll();
        return Task.FromResult(employees);
    }
}