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
        var employee = await employeeRepository.GetByIdWithManagedEmployeesAsync(request.EmployeeId, cancellationToken);

        if (employee is null)
        {
            return Result.Failure<EmployeeResponse>(new Error(
                Domain.ErrorCodes.Employee.NotFound,
                $"The employee with Id {request.EmployeeId} was not found"));
        }

        var response = mapper.Map<EmployeeResponse>(employee);

        return response;
    }
}