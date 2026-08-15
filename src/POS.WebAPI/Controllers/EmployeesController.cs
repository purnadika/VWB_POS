using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.Employees.Commands;
using POS.Application.Features.Employees.Queries;
using POS.Application.Features.Employees.DTOs;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/employees")]
// [Authorize(Roles = "Admin, Cashier")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetEmployeesQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeDto dto)
    {
        var command = new CreateEmployeeCommand { Dto = dto };
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Created($"/api/employees/{result.Value}", result.Value);
        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeDto dto)
    {
        var command = new UpdateEmployeeCommand { Id = id, Dto = dto };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteEmployeeCommand { Id = id };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}

