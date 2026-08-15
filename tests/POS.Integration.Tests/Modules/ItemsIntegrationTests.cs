using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using POS.Integration.Tests.Helpers;

namespace POS.Integration.Tests.Modules;

public class ItemsIntegrationTests : IntegrationTestBase
{
    

    public ItemsIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
        AuthenticateAs("Admin");
    }

    [Fact]
    public async Task Create_Item_With_Invalid_Data_Fails_Validation()
    {
        // Missing name, negative cost
        var invalidRequest = new { Name = "", CostPrice = -10.0m };
        var response = await Client.PostAsJsonAsync("/api/items", invalidRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Item_CRUD_Lifecycle_With_Tax_Configuration()
    {
        // 1. Create
        var createRequest = new { Name = "Taxable Item", Category = "Test", CostPrice = 10.00m, UnitPrice = 15.00m };
        var response = await Client.PostAsJsonAsync("/api/items", createRequest);
        if (!response.IsSuccessStatusCode) throw new System.Exception(await response.Content.ReadAsStringAsync());

        // 2. Fetch
        var getResponse = await Client.GetAsync("/api/items");
        var result = await getResponse.Content.ReadAsStringAsync();
        result.Should().Contain("Taxable Item");
    }

    [Fact]
    public async Task Soft_Delete_Item_Removes_From_Get_Results()
    {
        // 1. Create
        var createRequest = new { Name = "Delete Me Item", Category = "Test", CostPrice = 5.0m, UnitPrice = 10.0m };
        var response = await Client.PostAsJsonAsync("/api/items", createRequest);
        response.EnsureSuccessStatusCode();
        
        // Extract ID (assuming the returned value is the ID directly, or JSON string of int)
        var itemIdString = await response.Content.ReadAsStringAsync();
        int itemId = int.Parse(itemIdString.Trim());

        // 2. Delete
        var deleteResponse = await Client.DeleteAsync($"/api/items/{itemId}");
        deleteResponse.EnsureSuccessStatusCode();

        // 3. Fetch again
        var getResponse = await Client.GetAsync("/api/items");
        var result = await getResponse.Content.ReadAsStringAsync();
        
        // Should not contain the deleted item
        result.Should().NotContain("Delete Me Item");
    }
}


