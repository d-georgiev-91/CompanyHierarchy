namespace CompanyHierarchy.Presentation.Models;

public class EmployeeWithManagedEmployees : Employee
{
    public IEnumerable<EmployeeWithManagedEmployees> ManagedEmployees { get; set; } =
        [];
}