using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CompanyHierarchy.Domain.Commands;
using CompanyHierarchy.Presentation.Models;
using CompanyHierarchy.Domain.Queries;

namespace CompanyHierarchy.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController(ISender sender, IMapper mapper) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Put(Employee employee)
    {
        var upsertEmployeeCommand = mapper.Map<UpsertEmployeeCommand>(employee);
        var result = await sender.Send(upsertEmployeeCommand);

        if (upsertEmployeeCommand.EmployeeId == default)
        {
            return CreatedAtAction(nameof(Get), new { EmployeeId = result });
        }

        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var query = new GetEmployeeWithManagedEmployeesQuery { EmployeeId = id };
        var employee = await sender.Send(query);

        if (employee == null)
        {
            return NotFound();
        }

        var result = mapper.Map<EmployeeWithManagedEmployees>(employee);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var employees = await sender.Send(new GetAllEmployeesWithManagedEmployeesQuery());

        var employeeModels = mapper.Map<IEnumerable<EmployeeWithManagedEmployees>>(employees);

        return Ok(employeeModels);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await sender.Send(new DeleteEmployeeCommand { EmployeeId = id});
        return NoContent();
    }
}