using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.Expenses.Commands;
using POS.Application.Features.Expenses.Queries;
using POS.Application.Features.Expenses.DTOs;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/expenses")]
// [Authorize(Roles = "Admin, Cashier")]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpensesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetExpensesQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExpenseCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Created($"/api/expenses/{result.Value}", result.Value);
        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ExpenseDto dto)
    {
        var command = new UpdateExpenseCommand { Id = id, Dto = dto };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteExpenseCommand { Id = id };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}

