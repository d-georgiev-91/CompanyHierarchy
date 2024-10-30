using CompanyHierarchy.Domain.Entities;

namespace CompanyHierarchy.Domain.Repositories;

public interface IEmployeeRepository
{
    int Add(Employee employee);

    void Update(Employee employee);

    void Delete(int id);

    Employee? GetByIdWithManagedEmployees(int id);

    IEnumerable<Employee> GetAll();

    bool Exists(int id);
}