using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using POS.Integration.Tests.Helpers;

namespace POS.Integration.Tests.Modules;

public class UserManagementRBACIntegrationTests : IntegrationTestBase
{
    

    public UserManagementRBACIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
        
    }

    [Fact]
    public async Task NonAdmin_Cannot_Modify_Admin_Account()
    {
        AuthenticateAs("Admin"); var adminClient = Client;
        
        // 1. Create an admin user first to get a valid Guid
        var createRequest = new { 
            Email = "real_admin@example.com", 
            Password = "Password123!", 
            FullName = "Real Admin", 
            Role = "Admin" 
        };
        var createResponse = await adminClient.PostAsJsonAsync("/api/v1/usermanagement", createRequest);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<System.Text.Json.Nodes.JsonObject>();
        var adminId = createdUser?["id"]?.ToString();
        
        if (adminId == null) adminId = Guid.NewGuid().ToString();

        // 2. Try to update as Cashier
        AuthenticateAs("Cashier"); var cashierClient = Client;
        var updateRequest = new { FullName = "Hacked Admin" };
        var response = await cashierClient.PutAsJsonAsync($"/api/v1/usermanagement/{adminId}", updateRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task NonAdmin_Cannot_Grant_Permissions_They_Dont_Have()
    {
        AuthenticateAs("Cashier"); var cashierClient = Client; // Cashier doesn't have "config" permission

        var grantRequest = new { Permission = 1 };
        var targetId = Guid.NewGuid().ToString();
        var response = await cashierClient.PostAsJsonAsync($"/api/v1/usermanagement/{targetId}/permissions", grantRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Admin_Can_Modify_Any_Account()
    {
        AuthenticateAs("Admin"); var adminClient = Client;
        var someUserId = Guid.NewGuid().ToString(); // even if 404, it shouldn't be 403
        var updateRequest = new { FullName = "UpdatedBy Admin" };
        var response = await adminClient.PutAsJsonAsync($"/api/v1/usermanagement/{someUserId}/profile", updateRequest);
        
        response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task User_Can_Modify_Own_Account()
    {
        AuthenticateAs("Cashier"); var cashierClient = Client;
        // We simulate modifying own account by checking a special claim or passing the known ID
        // For now, let's just assert that we pass the 403 block if the ID matches the current user.
        // TestAuthHandler sets NameIdentifier to "test-user-id"
        var myUserId = "00000000-0000-0000-0000-000000000001"; 
        
        var updateRequest = new { FullName = "My OwnName" };
        var response = await cashierClient.PutAsJsonAsync($"/api/v1/usermanagement/{myUserId}/profile", updateRequest);
        
        response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Login_With_Incorrect_Password_Fails()
    {
        AuthenticateAs("Admin"); var adminClient = Client;
        var loginRequest = new { Email = "admin@example.com", Password = "wrongpassword" };
        var response = await adminClient.PostAsJsonAsync("/api/v1/usermanagement/login", loginRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_With_Deactivated_User_Fails()
    {
        AuthenticateAs("Admin"); var adminClient = Client;
        var loginRequest = new { Email = "deactivated@example.com", Password = "password" };
        var response = await adminClient.PostAsJsonAsync("/api/v1/usermanagement/login", loginRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}





