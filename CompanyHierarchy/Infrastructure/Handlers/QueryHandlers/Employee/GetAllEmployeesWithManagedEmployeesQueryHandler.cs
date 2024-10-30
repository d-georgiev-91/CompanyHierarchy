using AutoMapper;
using CompanyHierarchy.Domain.Common;
using CompanyHierarchy.Domain.Queries.Employee;
using CompanyHierarchy.Domain.Repositories;

namespace CompanyHierarchy.Infrastructure.Handlers.QueryHandlers.Employee;

public class GetAllEmployeesWithManagedEmployeesQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper) :
    IQueryHandler<GetAllEmployeesWithManagedEmployeesQuery, IEnumerable<EmployeeResponse>>
{
    public async Task<Result<IEnumerable<EmployeeResponse>>> Handle(GetAllEmployeesWithManagedEmployeesQuery request, CancellationToken cancellationToken)
    {
        var response = mapper.Map<IEnumerable<EmployeeResponse>>(await employeeRepository.GetAllAsync(cancellationToken));

        return Result.Success(response);
    }
}