using System.Net;
using FluentAssertions;
using Xunit;

namespace POS.Integration.Tests.Modules;

public class MessagesIntegrationTests : IntegrationTestBase
{
    public MessagesIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetMessages_WithoutAuth_ReturnsUnauthorized()
    {
        ClearAuthentication();
        var response = await Client.GetAsync("/api/messages");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
