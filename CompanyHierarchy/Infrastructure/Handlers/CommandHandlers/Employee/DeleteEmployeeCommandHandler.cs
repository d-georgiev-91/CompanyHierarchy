using CompanyHierarchy.Domain.Commands.Employee;
using CompanyHierarchy.Domain.Common;
using CompanyHierarchy.Domain.Repositories;
using CompanyHierarchy.Infrastructure.Handlers.QueryHandlers.Employee;

namespace CompanyHierarchy.Infrastructure.Handlers.CommandHandlers.Employee;

public class DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    : ICommandHandler<DeleteEmployeeCommand>
{
    public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await employeeRepository.DeleteAsync(request.EmployeeId, cancellationToken);
        }
        catch (InvalidOperationException)
        {
            // TODO: Log deletion failed into future logger
            return Result.Failure<EmployeeResponse>(new Error(
                Domain.ErrorCodes.Employee.InvalidOperation,
                $"The employee with Id {request.EmployeeId} was not removed"));
        }

        return Result.Success();
    }
}