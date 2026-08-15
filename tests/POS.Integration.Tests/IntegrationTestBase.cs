using System.Net.Http;
using System.Net.Http.Headers;
using Npgsql;
using Respawn;
using Xunit;

namespace POS.Integration.Tests;

public class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly HttpClient Client;
    private Respawner? _respawner;

    public IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
    }

    public virtual async Task InitializeAsync()
    {
        // Set up respawner if not initialized
        if (_respawner == null)
        {
            var conn = new NpgsqlConnection(Factory.GetConnectionString());
            await conn.OpenAsync();
            _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[] { "public" }
            });
            await conn.CloseAsync();
        }
    }

    public virtual async Task DisposeAsync()
    {
        // Reset DB after each test
        var conn = new NpgsqlConnection(Factory.GetConnectionString());
        await conn.OpenAsync();
        if (_respawner != null)
        {
            await _respawner.ResetAsync(conn);
        }
        await conn.CloseAsync();
    }

    /// <summary>
    /// Helper to attach a role to the HTTP Request to test Authorization.
    /// Uses our TestAuthHandler logic.
    /// </summary>
    protected void AuthenticateAs(string role)
    {
        Client.DefaultRequestHeaders.Remove("X-Test-Role");
        Client.DefaultRequestHeaders.Add("X-Test-Role", role);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Helpers.TestAuthHandler.DefaultScheme);
    }

    protected void ClearAuthentication()
    {
        Client.DefaultRequestHeaders.Remove("X-Test-Role");
        Client.DefaultRequestHeaders.Authorization = null;
    }
}
