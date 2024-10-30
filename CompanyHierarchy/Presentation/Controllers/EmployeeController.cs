using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CompanyHierarchy.Presentation.Models;
using CompanyHierarchy.Domain.Queries.Employee;
using CompanyHierarchy.Domain.Commands.Employee;

namespace CompanyHierarchy.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController(ISender sender, IMapper mapper) : ControllerBase
{
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(Employee employee, CancellationToken cancellationToken)
    {
        var upsertEmployeeCommand = mapper.Map<UpsertEmployeeCommand>(employee);
        var result = await sender.Send(upsertEmployeeCommand, cancellationToken);

        switch (result)
        {
            case { IsFailure: true, Error.Code: Domain.ErrorCodes.Employee.NotFound }:
                return NotFound(result.Error);
            case { IsFailure: true, Error.Code: Domain.ErrorCodes.Employee.InvalidOperation }:
                return BadRequest(result.Error);
        }

        if (employee.EmployeeId == default)
        {
            return CreatedAtAction(nameof(Get), new { EmployeeId = result.Value });
        }

        return Ok();
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var query = new GetEmployeeWithManagedEmployeesQuery { EmployeeId = id };
        var result = await sender.Send(query, cancellationToken);

        if (result is { IsFailure: true, Error.Code: Domain.ErrorCodes.Employee.NotFound })
        {
            return NotFound(result.Error);
        }

        var employees = mapper.Map<EmployeeWithManagedEmployees>(result.Value);
        return Ok(employees);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllEmployeesWithManagedEmployeesQuery(), cancellationToken);

        var response = mapper.Map<IEnumerable<EmployeeWithManagedEmployees>>(result.Value);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteEmployeeCommand { EmployeeId = id }, cancellationToken);

        if (result is { IsFailure: true, Error.Code: Domain.ErrorCodes.Employee.InvalidOperation })
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }
}