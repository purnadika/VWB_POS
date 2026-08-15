using System.Net;
using FluentAssertions;
using Xunit;

namespace POS.Integration.Tests.Modules;

public class ExpensesIntegrationTests : IntegrationTestBase
{
    public ExpensesIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetExpenses_WithoutAuth_ReturnsUnauthorized()
    {
        ClearAuthentication();
        var response = await Client.GetAsync("/api/expenses");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
