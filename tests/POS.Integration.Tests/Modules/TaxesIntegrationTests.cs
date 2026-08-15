using System.Net;
using FluentAssertions;
using Xunit;

namespace POS.Integration.Tests.Modules;

public class TaxesIntegrationTests : IntegrationTestBase
{
    public TaxesIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetTaxes_WithoutAuth_ReturnsUnauthorized()
    {
        ClearAuthentication();
        var response = await Client.GetAsync("/api/taxes");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
