using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.Reports.Queries;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Admin, Cashier")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("summary/taxes")]
    public async Task<IActionResult> GetTaxSummary()
    {
        var result = await _mediator.Send(new GetTaxSummaryQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpGet("summary/discounts")]
    public async Task<IActionResult> GetDiscountSummary()
    {
        var result = await _mediator.Send(new GetDiscountSummaryQuery());
        if (result.IsSuccess) return Ok(result.Value);
        return BadRequest(result.Error);
    }
}
