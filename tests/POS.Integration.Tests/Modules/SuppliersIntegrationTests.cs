using System.Net;
using FluentAssertions;
using Xunit;

namespace POS.Integration.Tests.Modules;

public class SuppliersIntegrationTests : IntegrationTestBase
{
    public SuppliersIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetSuppliers_WithoutAuth_ReturnsUnauthorized()
    {
        ClearAuthentication();
        var response = await Client.GetAsync("/api/suppliers");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
