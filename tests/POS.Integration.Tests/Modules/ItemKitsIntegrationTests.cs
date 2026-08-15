using System.Net;
using FluentAssertions;
using Xunit;

namespace POS.Integration.Tests.Modules;

public class ItemKitsIntegrationTests : IntegrationTestBase
{
    public ItemKitsIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetItemKits_WithoutAuth_ReturnsUnauthorized()
    {
        ClearAuthentication();
        var response = await Client.GetAsync("/api/itemkits");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
