using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace POS.UI.Tests.AI;

/// <summary>
/// AI helper that uses an IChatClient to generate dynamic edge-case test scenarios
/// and provide natural-language test data suggestions for Playwright UI tests.
/// </summary>
public class PlaywrightAiHelper
{
    private readonly IChatClient _chatClient;

    public PlaywrightAiHelper(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    /// <summary>
    /// Asks the AI to suggest an unusual but valid edge-case test scenario for checkout.
    /// </summary>
    public async Task<string> SuggestCheckoutEdgeCaseAsync(CancellationToken cancellationToken = default)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, "You are a QA engineer for a Point of Sale system. Suggest realistic edge-case checkout scenarios that would stress-test the system. Be specific and brief (1 sentence)."),
            new(ChatRole.User, "Give me one unusual but realistic checkout scenario I should test.")
        };

        var response = await _chatClient.CompleteAsync(messages, cancellationToken: cancellationToken);
        return response.Message.Text ?? "Test a sale with mixed payment methods (split Cash + Gift Card).";
    }

    /// <summary>
    /// Asks the AI to evaluate whether a UI response text looks like a valid receipt.
    /// </summary>
    public async Task<bool> EvaluateReceiptTextAsync(string receiptText, CancellationToken cancellationToken = default)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, "You are a QA validator for POS receipts. Reply only with 'VALID' or 'INVALID' based on whether the text contains key receipt fields: invoice number, line items, subtotal, tax, and total."),
            new(ChatRole.User, receiptText)
        };

        var response = await _chatClient.CompleteAsync(messages, cancellationToken: cancellationToken);
        var answer = response.Message.Text ?? "";
        return answer.Contains("VALID", System.StringComparison.OrdinalIgnoreCase);
    }
}
