using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.GiftCards.Commands;
using POS.Application.Features.GiftCards.Queries;
using POS.Application.Features.GiftCards.DTOs;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/giftcards")]
// [Authorize(Roles = "Admin, Cashier")]
public class GiftCardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GiftCardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetGiftCardsQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGiftcardCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Created($"/api/giftcards/{result.Value}", result.Value);
        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] GiftcardDto dto)
    {
        var command = new UpdateGiftcardCommand { Id = id, Dto = dto };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteGiftcardCommand { Id = id };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}

