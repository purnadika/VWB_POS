using System.Net;
using FluentAssertions;
using Xunit;

namespace POS.Integration.Tests.Modules;

public class GiftCardsIntegrationTests : IntegrationTestBase
{
    public GiftCardsIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetGiftCards_WithoutAuth_ReturnsUnauthorized()
    {
        ClearAuthentication();
        var response = await Client.GetAsync("/api/giftcards");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
