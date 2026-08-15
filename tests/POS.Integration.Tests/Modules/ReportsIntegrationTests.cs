using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using POS.Integration.Tests.Helpers;

namespace POS.Integration.Tests.Modules;

public class ReportsIntegrationTests : IntegrationTestBase
{
    

    public ReportsIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
        AuthenticateAs("Admin");
    }

    [Fact]
    public async Task Get_Tax_Summary_Report_Succeeds()
    {
        var response = await Client.GetAsync("/api/reports/summary/taxes");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        result.Should().Contain("tax_amount");
    }

    [Fact]
    public async Task Get_Discount_Summary_Report_Succeeds()
    {
        var response = await Client.GetAsync("/api/reports/summary/discounts");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        result.Should().Contain("discount_amount");
    }
}

