using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.Sales.Commands;
using POS.Application.Features.Sales.Queries;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISender _sender;

    public SalesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSaleCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _sender.Send(new GetSalesQuery());
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _sender.Send(new GetSaleQuery(id));
        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateSaleCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await _sender.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _sender.Send(new DeleteSaleCommand { Id = id });
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}

