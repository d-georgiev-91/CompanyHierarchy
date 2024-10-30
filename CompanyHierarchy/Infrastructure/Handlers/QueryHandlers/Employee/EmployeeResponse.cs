using CompanyHierarchy.Presentation.Models;

namespace CompanyHierarchy.Infrastructure.Handlers.QueryHandlers.Employee;

public class EmployeeResponse
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = default!;

    public string Title { get; set; } = default!;

    public int? ManagerEmployeeId { get; set; }

    public IEnumerable<EmployeeResponse> ManagedEmployees { get; set; } =
        [];
}