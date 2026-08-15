using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.Taxes.Commands;
using POS.Application.Features.Taxes.Queries;
using POS.Application.Features.Taxes.DTOs;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/taxes")]
// [Authorize(Roles = "Admin, Cashier")]
public class TaxesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaxesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetTaxesQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaxRateCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Created($"/api/taxes/{result.Value}", result.Value);
        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaxRateDto dto)
    {
        var command = new UpdateTaxRateCommand { Id = id, Dto = dto };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteTaxRateCommand { Id = id };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}

