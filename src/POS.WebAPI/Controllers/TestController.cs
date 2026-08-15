using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.Expenses.Commands;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] CreateExpenseCommand command)
    {
        return Ok(new {
            receivedAmount = command.Amount,
            receivedDescription = command.Description,
            commandType = command.GetType().Name
        });
    }
}
