namespace CompanyHierarchy.Domain.Entities;

public class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public int? ManagerEmployeeId { get; set; }

    public ICollection<Employee> ManagedEmployees { get; set; } = new HashSet<Employee>();
}