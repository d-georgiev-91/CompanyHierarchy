namespace CompanyHierarchy.Presentation.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = default!;

    public string Title { get; set; } = default!;

    public int? ManagerEmployeeId { get; set; }
}