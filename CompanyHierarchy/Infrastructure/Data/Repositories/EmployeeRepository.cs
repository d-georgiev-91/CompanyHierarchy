using System.Data;
using CompanyHierarchy.Domain.Entities;
using CompanyHierarchy.Domain.Repositories;

namespace CompanyHierarchy.Infrastructure.Data.Repositories;

public class EmployeeRepository(IDbConnectionFactory connectionFactory) : IEmployeeRepository
{
    private static void AddParameter(IDbCommand command, string parameterName, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = parameterName;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }

    public async Task<int> AddAsync(Employee employee, CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.CreateConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Employees (FullName, Title, ManagerEmployeeId)
            VALUES (@FullName, @Title, @ManagerEmployeeId)
            RETURNING EmployeeId;
        ";

        AddParameter(command, "@FullName", employee.FullName);
        AddParameter(command, "@Title", employee.Title);
        AddParameter(command, "@ManagerEmployeeId", employee.ManagerEmployeeId.HasValue ? employee.ManagerEmployeeId.Value : DBNull.Value);

        await connection.OpenAsync(cancellationToken);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        await connection.CloseAsync();
        var newEmployeeId = Convert.ToInt32(result);

        return newEmployeeId;
    }

    public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.CreateConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Employees
            SET FullName = @FullName,
                Title = @Title,
                ManagerEmployeeId = @ManagerEmployeeId
            WHERE EmployeeId = @EmployeeId;
        ";

        AddParameter(command, "@FullName", employee.FullName);
        AddParameter(command, "@Title", employee.Title);
        AddParameter(command, "@ManagerEmployeeId", employee.ManagerEmployeeId.HasValue ? employee.ManagerEmployeeId.Value : DBNull.Value);
        AddParameter(command, "@EmployeeId", employee.EmployeeId);

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
        await connection.CloseAsync();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            await using var reassignCommand = connection.CreateCommand();
            reassignCommand.Transaction = transaction;
            reassignCommand.CommandText = @"
                    UPDATE Employees
                    SET ManagerEmployeeId = (
                        SELECT ManagerEmployeeId FROM Employees WHERE EmployeeId = @EmployeeId
                    )
                    WHERE ManagerEmployeeId = @EmployeeId;
                ";
            AddParameter(reassignCommand, "@EmployeeId", id);
            await reassignCommand.ExecuteNonQueryAsync(cancellationToken);

            await using var deleteCommand = connection.CreateCommand();
            deleteCommand.Transaction = transaction;
            deleteCommand.CommandText = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId;";
            AddParameter(deleteCommand, "@EmployeeId", id);
            await deleteCommand.ExecuteNonQueryAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch(InvalidOperationException)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Employee?> GetByIdWithManagedEmployeesAsync(int id, CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.CreateConnection();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            WITH RECURSIVE EmployeeCTE AS (
                SELECT EmployeeId, FullName, Title, ManagerEmployeeId
                FROM Employees
                WHERE EmployeeId = @EmployeeId
                UNION ALL
                SELECT e.EmployeeId, e.FullName, e.Title, e.ManagerEmployeeId
                FROM Employees e
                INNER JOIN EmployeeCTE ec ON e.ManagerEmployeeId = ec.EmployeeId
            )
            SELECT * FROM EmployeeCTE;
        ";

        AddParameter(command, "@EmployeeId", id);

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var employees = new Dictionary<int, Employee>();

        while (await reader.ReadAsync(cancellationToken))
        {
            var employeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
            var fullName = reader.GetString(reader.GetOrdinal("FullName"));
            var title = reader.GetString(reader.GetOrdinal("Title"));
            int? managerEmployeeId = reader.IsDBNull(reader.GetOrdinal("ManagerEmployeeId"))
                ? null
                : reader.GetInt32(reader.GetOrdinal("ManagerEmployeeId"));

            if (!employees.TryGetValue(employeeId, out var employee))
            {
                employee = new Employee
                {
                    EmployeeId = employeeId,
                    FullName = fullName,
                    Title = title,
                    ManagerEmployeeId = managerEmployeeId,
                };
                employees[employeeId] = employee;
            }

            if (managerEmployeeId.HasValue)
            {
                if (!employees.TryGetValue(managerEmployeeId.Value, out var manager))
                {
                    manager = new Employee
                    {
                        EmployeeId = managerEmployeeId.Value,
                    };
                    employees[managerEmployeeId.Value] = manager;
                }
                manager.ManagedEmployees.Add(employee);
            }
        }

        employees.TryGetValue(id, out var rootEmployee);
        return rootEmployee;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            WITH RECURSIVE EmployeeHierarchy AS (
                SELECT 
                    EmployeeId, 
                    FullName, 
                    Title, 
                    ManagerEmployeeId,
                    ARRAY[EmployeeId] AS Path
                FROM Employees
                WHERE ManagerEmployeeId IS NULL

                UNION ALL

                SELECT 
                    e.EmployeeId, 
                    e.FullName, 
                    e.Title, 
                    e.ManagerEmployeeId,
                    eh.Path || e.EmployeeId
                FROM Employees e
                INNER JOIN EmployeeHierarchy eh ON e.ManagerEmployeeId = eh.EmployeeId
            )
            SELECT * FROM EmployeeHierarchy ORDER BY Path;
        ";

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var employees = new Dictionary<int, Employee>();

        while (await reader.ReadAsync(cancellationToken))
        {
            var employeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
            var fullName = reader.GetString(reader.GetOrdinal("FullName"));
            var title = reader.GetString(reader.GetOrdinal("Title"));
            int? managerEmployeeId = reader.IsDBNull(reader.GetOrdinal("ManagerEmployeeId"))
                ? null
                : reader.GetInt32(reader.GetOrdinal("ManagerEmployeeId"));

            var employee = new Employee
            {
                EmployeeId = employeeId,
                FullName = fullName,
                Title = title,
                ManagerEmployeeId = managerEmployeeId,
            };

            employees[employeeId] = employee;

            if (managerEmployeeId.HasValue && employees.TryGetValue(managerEmployeeId.Value, out var manager))
            {
                manager.ManagedEmployees.Add(employee);
            }
        }

        return employees.Values.Where(e => e.ManagerEmployeeId == null).ToList();
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.CreateConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT 1
            FROM Employees
            WHERE EmployeeId = @EmployeeId
            LIMIT 1;
        ";

        AddParameter(command, "@EmployeeId", id);

        await connection.OpenAsync(cancellationToken);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        await connection.CloseAsync();

        return result != null;
    }
}