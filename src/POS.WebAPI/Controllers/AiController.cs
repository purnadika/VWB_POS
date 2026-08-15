using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using POS.Application.AI;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiController : ControllerBase
{
    private readonly POSAssistantAgent _agent;

    public AiController(POSAssistantAgent agent)
    {
        _agent = agent;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message cannot be empty.");

        // Convert DTO conversation history to Microsoft.Extensions.AI ChatMessage objects
        var history = new List<ChatMessage>();
        if (request.History != null)
        {
            foreach (var h in request.History)
            {
                var role = h.IsUser ? ChatRole.User : ChatRole.Assistant;
                history.Add(new ChatMessage(role, h.Text));
            }
        }

        var responseText = await _agent.ChatAsync(request.Message, history);
        return Ok(new ChatResponseDto(responseText));
    }
}

public record ChatRequest(string Message, List<ChatHistoryDto>? History);
public record ChatHistoryDto(string Text, bool IsUser);
public record ChatResponseDto(string Response);
