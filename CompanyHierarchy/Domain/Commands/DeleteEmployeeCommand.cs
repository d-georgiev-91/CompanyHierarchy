using MediatR;

namespace CompanyHierarchy.Domain.Commands;

public class DeleteEmployeeCommand : IRequest
{
    public int EmployeeId { get; set; }
}