using System.Net;
using FluentAssertions;
using Xunit;

namespace POS.Integration.Tests.Modules;

public class EmployeesIntegrationTests : IntegrationTestBase
{
    public EmployeesIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetEmployees_WithoutAuth_ReturnsUnauthorized()
    {
        ClearAuthentication();
        var response = await Client.GetAsync("/api/employees");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
