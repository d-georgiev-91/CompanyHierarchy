namespace CompanyHierarchy.Domain.Commands.Employee;

public class DeleteEmployeeCommand : ICommand
{
    public int EmployeeId { get; set; }
}