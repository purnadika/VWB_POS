using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.Items.Commands;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly ISender _sender;

    public ItemsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateItemCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    [HttpPost("import")]
    public async Task<IActionResult> Import(Microsoft.AspNetCore.Http.IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("File is empty");
        using var stream = file.OpenReadStream();
        var command = new POS.Application.Features.Items.Commands.ImportItemsCommand(stream);
        var result = await _sender.Send(command);
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _sender.Send(new POS.Application.Features.Items.Queries.GetItemsQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateItemCommand command)
    {
        command.Id = id;
        var result = await _sender.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _sender.Send(new DeleteItemCommand { Id = id });
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
