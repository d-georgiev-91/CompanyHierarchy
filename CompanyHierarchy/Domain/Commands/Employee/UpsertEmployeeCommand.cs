namespace CompanyHierarchy.Domain.Commands.Employee;

public class UpsertEmployeeCommand : ICommand<int>
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = default!;

    public string Title { get; set; } = default!;

    public int? ManagerEmployeeId { get; set; }
}