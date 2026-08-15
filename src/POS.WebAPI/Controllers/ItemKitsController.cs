using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.ItemKits.Commands;
using POS.Application.Features.ItemKits.Queries;
using POS.Application.Features.ItemKits.DTOs;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/itemkits")]
// [Authorize(Roles = "Admin, Cashier")]
public class ItemKitsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemKitsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetItemKitsQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ItemKitDto dto)
    {
        var command = new CreateItemKitCommand { Dto = dto };
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Created($"/api/itemkits/{result.Value}", result.Value);
        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ItemKitDto dto)
    {
        var command = new UpdateItemKitCommand { Id = id, Dto = dto };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteItemKitCommand { Id = id };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}

