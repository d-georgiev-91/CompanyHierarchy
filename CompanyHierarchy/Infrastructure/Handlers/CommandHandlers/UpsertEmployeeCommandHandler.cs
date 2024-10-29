using AutoMapper;
using CompanyHierarchy.Domain.Commands;
using CompanyHierarchy.Domain.Entities;
using CompanyHierarchy.Domain.Repositories;
using MediatR;

namespace CompanyHierarchy.Infrastructure.Handlers.CommandHandlers;

public class UpsertEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    : IRequestHandler<UpsertEmployeeCommand, int>
{
    public Task<int> Handle(UpsertEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = mapper.Map<Employee>(request);

        if (request.EmployeeId == default)
        {
            return Task.FromResult(employeeRepository.Add(employee));
        }

        employeeRepository.Update(employee);

        return Task.FromResult(employee.EmployeeId);
    }
}