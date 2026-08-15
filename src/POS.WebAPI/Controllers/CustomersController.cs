using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.Customers.Commands;
using POS.Application.Features.Customers.Queries;
using POS.Application.Features.Customers.DTOs;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Admin, Cashier")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetCustomersQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerDto dto)
    {
        var command = new CreateCustomerCommand { Dto = dto };
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Created($"/api/customers/{result.Value}", result.Value);
        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CustomerDto dto)
    {
        var command = new UpdateCustomerCommand { Id = id, Dto = dto };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteCustomerCommand { Id = id };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
    [HttpPost("import")]
    public async Task<IActionResult> Import(Microsoft.AspNetCore.Http.IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("File is empty");
        using var stream = file.OpenReadStream();
        var command = new POS.Application.Features.Customers.Commands.ImportCustomersCommand(stream);
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }
}
