using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace POS.Application.AI;

public class POSAssistantAgent
{
    private readonly IChatClient _chatClient;
    private readonly POSAssistantTools _tools;

    public POSAssistantAgent(IChatClient chatClient, POSAssistantTools tools)
    {
        _chatClient = chatClient;
        _tools = tools;
    }

    public async Task<string> ChatAsync(string userMessage, List<ChatMessage> conversationHistory, CancellationToken cancellationToken = default)
    {
        // Define system instructions
        var systemMessage = new ChatMessage(ChatRole.System, 
            "You are a helpful, professional, and knowledgeable Point of Sale (POS) Assistant. " +
            "You help store staff and managers query inventory levels, analyze sales trends, and draft purchase orders. " +
            "Always be concise and direct. Use the provided tools to fetch facts before answering.");

        var messages = new List<ChatMessage> { systemMessage };
        messages.AddRange(conversationHistory);
        messages.Add(new ChatMessage(ChatRole.User, userMessage));

        // Map POSAssistantTools methods to AIFunction objects
        var chatTools = new List<AITool>
        {
            AIFunctionFactory.Create(_tools.GetInventoryStatus, nameof(_tools.GetInventoryStatus)),
            AIFunctionFactory.Create(_tools.GetSalesSummary, nameof(_tools.GetSalesSummary)),
            AIFunctionFactory.Create(_tools.DraftPurchaseOrder, nameof(_tools.DraftPurchaseOrder))
        };

        var options = new ChatOptions
        {
            Tools = chatTools,
            ToolMode = ChatToolMode.Auto
        };

        // Call the chat client (Ollama/OpenAI etc.)
        var response = await _chatClient.CompleteAsync(messages, options, cancellationToken);
        return response.Message.Text ?? "I'm sorry, I couldn't process that request.";
    }
}
