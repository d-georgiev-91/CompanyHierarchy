using CompanyHierarchy.Domain.Entities;

namespace CompanyHierarchy.Domain.Repositories;

public interface IEmployeeRepository
{
    Task<int> AddAsync(Employee employee, CancellationToken cancellationToken);

    Task UpdateAsync(Employee employee, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);

    Task<Employee?> GetByIdWithManagedEmployeesAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
}