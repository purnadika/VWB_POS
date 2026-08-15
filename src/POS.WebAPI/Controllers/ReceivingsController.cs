using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.Receivings.Commands;
using POS.Application.Features.Receivings.Queries;
using POS.Application.Features.Receivings.DTOs;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/receivings")]
// [Authorize(Roles = "Admin, Cashier")]
public class ReceivingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReceivingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetReceivingsQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReceivingCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Created($"/api/receivings/{result.Value}", result.Value);
        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ReceivingDto dto)
    {
        var command = new UpdateReceivingCommand { Id = id, Dto = dto };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteReceivingCommand { Id = id };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}

