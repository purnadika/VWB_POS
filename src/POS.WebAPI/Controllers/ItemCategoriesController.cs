using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.ItemCategories.Commands;
using POS.Application.Features.ItemCategories.DTOs;
using POS.Application.Features.ItemCategories.Queries;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/item-categories")]
public class ItemCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetItemCategoriesQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Created($"/api/item-categories/{result.Value}", result.Value);
        return BadRequest(new { error = result.Error });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ItemCategoryDto dto)
    {
        var result = await _mediator.Send(new UpdateItemCategoryCommand(id, dto));
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(new { error = result.Error });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteItemCategoryCommand(id));
        if (result.IsSuccess) return Ok();
        return BadRequest(new { error = result.Error });
    }
}
