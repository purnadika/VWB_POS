using System.Data.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using POS.Infrastructure.Persistence;
using Testcontainers.PostgreSql;
using Xunit;

namespace POS.Integration.Tests;

public static class WebApplicationFactoryExtensions { public static HttpClient CreateClientWithAuth(this CustomWebApplicationFactory factory, string role) { var client = factory.CreateClient(); client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("TestAuth", role); return client; } }

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("pos_integration_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public string GetConnectionString() => _dbContainer.GetConnectionString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Remove the existing DbContext configuration
            services.RemoveAll(typeof(DbContextOptions<POSDbContext>));
            services.RemoveAll(typeof(DbConnection));

            // Add PostgreSQL configured for the test container
            services.AddDbContext<POSDbContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));

            // Add Test Authentication
            services.AddAuthentication(Helpers.TestAuthHandler.DefaultScheme)
                .AddScheme<AuthenticationSchemeOptions, Helpers.TestAuthHandler>(
                    Helpers.TestAuthHandler.DefaultScheme, options => { });
            
            // Allow tests to bypass authorization requirements if configured generically, 
            // but we will enforce attributes in the controllers to ensure tests catch AuthZ failures.
        });
        
        // Ensure tests run in Testing environment
        builder.UseEnvironment("Testing");
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        // Ensure schema is created
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<POSDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}

