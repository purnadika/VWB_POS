using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using POS.Integration.Tests.Helpers;

namespace POS.Integration.Tests.Modules;

public class CsvImportIntegrationTests : IntegrationTestBase
{
    

    public CsvImportIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
        AuthenticateAs("Admin");
    }

    [Fact]
    public async Task Import_Customers_From_CSV_Succeeds()
    {
        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("FirstName,LastName,Email\nTest,User,test.csv@example.com"));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        content.Add(fileContent, "file", "customers.csv");

        var response = await Client.PostAsync("/api/customers/import", content);
        
        response.EnsureSuccessStatusCode();
        var getResponse = await Client.GetAsync("/api/customers");
        var result = await getResponse.Content.ReadAsStringAsync();
        result.Should().Contain("test.csv@example.com");
    }

    [Fact]
    public async Task Import_Items_From_CSV_Succeeds()
    {
        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("Name,Category,CostPrice,UnitPrice\nCsvItem,TestCat,10.00,15.00"));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        content.Add(fileContent, "file", "items.csv");

        var response = await Client.PostAsync("/api/items/import", content);
        
        response.EnsureSuccessStatusCode();
        var getResponse = await Client.GetAsync("/api/items");
        var result = await getResponse.Content.ReadAsStringAsync();
        result.Should().Contain("CsvItem");
    }
}

