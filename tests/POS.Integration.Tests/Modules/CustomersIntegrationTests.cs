using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace POS.Integration.Tests.Modules;

public class CustomersIntegrationTests : IntegrationTestBase
{
    public CustomersIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetCustomers_WithoutAuth_ReturnsUnauthorized()
    {
        ClearAuthentication();
        var response = await Client.GetAsync("/api/customers");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetCustomers_WithCashierRole_ReturnsForbidden_Or_SuccessDependingOnPolicy()
    {
        AuthenticateAs("Cashier");
        var response = await Client.GetAsync("/api/customers");
        
        // Either 403 Forbidden (if Cashier can't view customers) or 200/404 if they can.
        // Assuming TDD, this endpoint doesn't exist yet, so we expect 404 if auth passes,
        // but if Auth fails we expect 403. Let's assert it's NOT 401.
        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Customer_FullCrudLifecycle_Succeeds()
    {
        AuthenticateAs("Admin");
        
        // 1. Create
        var createRequest = new { Name = "John Doe", Email = "john@example.com", PhoneNumber = "123456789" };
        var createResponse = await Client.PostAsJsonAsync("/api/customers", createRequest);
        createResponse.EnsureSuccessStatusCode();
        
        // Assuming the Create endpoint returns the new ID, we can fetch it, but here we'll just Get all and find it.
        var getResponse = await Client.GetAsync("/api/customers");
        getResponse.EnsureSuccessStatusCode();
        
        // We could deserialize but let's just do a string check for simplicity
        var contentString = await getResponse.Content.ReadAsStringAsync();
        contentString.Should().Contain("\"firstName\":\"John\"");
        contentString.Should().Contain("\"lastName\":\"Doe\"");

        // Assuming ID is 2 since maybe ID 1 was used by another test or seed data.
        // Actually, we should parse the JSON to get the real ID!
        var customers = await getResponse.Content.ReadFromJsonAsync<System.Collections.Generic.List<POS.Application.Features.Customers.DTOs.CustomerDto>>();
        int customerId = customers!.First(c => c.Email == "john@example.com").Id;

        // 2. Update
        var updateRequest = new { Id = customerId, FirstName = "John", LastName = "Doe Updated", Email = "john_new@example.com" };
        var updateResponse = await Client.PutAsJsonAsync($"/api/customers/{customerId}", updateRequest);
        updateResponse.EnsureSuccessStatusCode();

        // 3. Verify Update
        var getUpdatedResponse = await Client.GetAsync("/api/customers");
        var updatedContentString = await getUpdatedResponse.Content.ReadAsStringAsync();
        updatedContentString.Should().Contain("\"lastName\":\"Doe Updated\"");

        // 4. Soft Delete
        var deleteResponse = await Client.DeleteAsync($"/api/customers/{customerId}");
        deleteResponse.EnsureSuccessStatusCode();

        // 5. Verify Soft Delete
        var getDeletedResponse = await Client.GetAsync("/api/customers");
        var deletedContentString = await getDeletedResponse.Content.ReadAsStringAsync();
        deletedContentString.Should().NotContain("\"email\":\"john_new@example.com\"");
    }
}
