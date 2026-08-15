using System.Net;
using FluentAssertions;
using Xunit;

namespace POS.Integration.Tests.Modules;

public class ReceivingIntegrationTests : IntegrationTestBase
{
    public ReceivingIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetReceivings_WithoutAuth_ReturnsUnauthorized()
    {
        ClearAuthentication();
        var response = await Client.GetAsync("/api/receivings");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
