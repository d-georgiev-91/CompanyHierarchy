using MediatR;

namespace CompanyHierarchy.Domain.Commands;

public class UpsertEmployeeCommand : IRequest<int>
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = default!;

    public string Title { get; set; } = default!;

    public int? ManagerEmployeeId { get; set; }
}