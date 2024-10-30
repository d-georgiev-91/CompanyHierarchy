using AutoMapper;
using CompanyHierarchy.Domain.Common;
using CompanyHierarchy.Domain.Queries.Employee;
using CompanyHierarchy.Domain.Repositories;

namespace CompanyHierarchy.Infrastructure.Handlers.QueryHandlers.Employee;

public class GetEmployeeWithManagedEmployeesQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    : IQueryHandler<GetEmployeeWithManagedEmployeesQuery, EmployeeResponse>
{
    public async Task<Result<EmployeeResponse>> Handle(GetEmployeeWithManagedEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employee = employeeRepository.GetByIdWithManagedEmployees(request.EmployeeId);

        if (employee is null)
        {
            return await Task.FromResult(Result.Failure<EmployeeResponse>(new Error(
                Domain.ErrorCodes.Employee.NotFound,
                $"The employee with Id {request.EmployeeId} was not found")));
        }

        var response = mapper.Map<EmployeeResponse>(employee);
        
        return await Task.FromResult(response);
    }
}