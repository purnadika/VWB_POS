using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/messages")]
// [Authorize(Roles = "Admin, Cashier")]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new POS.Application.Features.Messages.Queries.GetMessagesQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] POS.Application.Features.Messages.Commands.CreateMessageCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Created($"/api/messages/{result.Value}", result.Value);
        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] POS.Application.Features.Messages.DTOs.MessageDto dto)
    {
        var command = new POS.Application.Features.Messages.Commands.UpdateMessageCommand { Id = id, Dto = dto };
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new POS.Application.Features.Messages.Commands.DeleteMessageCommand { Id = id };
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Ok();
        return BadRequest(result.Error);
    }
}

