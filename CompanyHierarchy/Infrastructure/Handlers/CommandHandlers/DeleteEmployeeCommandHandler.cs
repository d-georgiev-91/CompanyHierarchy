using AutoMapper;
using CompanyHierarchy.Domain.Commands;
using CompanyHierarchy.Domain.Repositories;
using MediatR;

namespace CompanyHierarchy.Infrastructure.Handlers.CommandHandlers;

public class DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    : IRequestHandler<DeleteEmployeeCommand>
{
    public Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        employeeRepository.Delete(request.EmployeeId);

        return Task.CompletedTask;
    }
}