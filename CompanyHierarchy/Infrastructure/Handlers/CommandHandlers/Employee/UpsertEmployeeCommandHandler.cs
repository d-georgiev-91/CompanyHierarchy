using AutoMapper;
using CompanyHierarchy.Domain.Commands.Employee;
using CompanyHierarchy.Domain.Common;
using CompanyHierarchy.Domain.Repositories;

namespace CompanyHierarchy.Infrastructure.Handlers.CommandHandlers.Employee;

public class UpsertEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    : ICommandHandler<UpsertEmployeeCommand, int>
{
    public async Task<Result<int>> Handle(UpsertEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = mapper.Map<Domain.Entities.Employee>(request);

        if (request.ManagerEmployeeId.HasValue && !await employeeRepository.ExistsAsync(request.ManagerEmployeeId.Value, cancellationToken))
        {
            return Result.Failure<int>(new Error(
                Domain.ErrorCodes.Employee.InvalidOperation,
                $"Manager with Id {request.ManagerEmployeeId} does not exist"));
        }

        if (request.EmployeeId == default)
        {
            return await employeeRepository.AddAsync(employee, cancellationToken);
        }

        if (!await employeeRepository.ExistsAsync(employee.EmployeeId, cancellationToken))
        {
            return Result.Failure<int>(new Error(
                Domain.ErrorCodes.Employee.NotFound,
                $"The employee with Id {request.EmployeeId} was not found"));
        }

        await employeeRepository.UpdateAsync(employee, cancellationToken);

        return employee.EmployeeId;
    }
}