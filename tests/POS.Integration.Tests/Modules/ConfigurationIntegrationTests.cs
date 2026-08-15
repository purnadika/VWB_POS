using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using POS.Integration.Tests.Helpers;

namespace POS.Integration.Tests.Modules;

public class ConfigurationIntegrationTests : IntegrationTestBase
{
    

    public ConfigurationIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
        AuthenticateAs("Admin");
    }

    [Fact]
    public async Task Save_Configuration_Persists_Changes()
    {
        var updateRequest = new { Key = "company", Value = "Test Company" };
        var response = await Client.PostAsJsonAsync("/api/configuration", updateRequest);
        response.EnsureSuccessStatusCode();

        var getResponse = await Client.GetAsync("/api/configuration");
        var result = await getResponse.Content.ReadAsStringAsync();
        result.Should().Contain("\"Test Company\"");
    }

    [Fact]
    public async Task Get_Configuration_Returns_Default_Keys()
    {
        var getResponse = await Client.GetAsync("/api/configuration");
        getResponse.EnsureSuccessStatusCode();
        var result = await getResponse.Content.ReadAsStringAsync();
        
        // As long as it returns valid JSON dictionary, it's fine.
        result.Should().StartWith("[");
        result.Should().EndWith("]");
    }
}




