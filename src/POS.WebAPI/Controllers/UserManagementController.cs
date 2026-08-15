using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Constants;
using POS.Application.Features.UserManagement.Commands;
using POS.Application.Features.UserManagement.DTOs;
using POS.Application.Features.UserManagement.Queries;
using POS.Domain.Enums;

namespace POS.WebAPI.Controllers;

/// <summary>
/// API Controller for User Management operations.
/// Implements REST conventions with proper HTTP semantics.
/// All endpoints support multi-language responses via Accept-Language header.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class UserManagementController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserManagementController> _logger;

    public UserManagementController(IMediator mediator, ILogger<UserManagementController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get a user by ID.
    /// </summary>
    /// <param name="id">The user ID (GUID)</param>
    /// <returns>The user details</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUser(
        Guid id,
        [FromHeader(Name = "Accept-Language")] string? locale = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetUserQuery
            {
                UserId = id,
                PreferredLanguage = locale
            };

            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning($"User not found: {id}");
                return NotFound(new { error = result.Message });
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving user {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An error occurred while retrieving the user" });
        }
    }

    /// <summary>
    /// Get a user by email address.
    /// </summary>
    /// <param name="email">The user's email address</param>
    /// <returns>The user details</returns>
    [HttpGet("by-email")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserByEmail(
        [FromQuery(Name = "email")] string email,
        [FromHeader(Name = "Accept-Language")] string? locale = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new { error = "Email is required" });
        }

        try
        {
            var query = new GetUserByEmailQuery
            {
                Email = email,
                PreferredLanguage = locale
            };

            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning($"User not found by email: {email}");
                return NotFound(new { error = result.Message });
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving user by email: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An error occurred while retrieving the user" });
        }
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <param name="request">The user creation request with email, password, role, etc.</param>
    /// <returns>The created user details</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request,
        [FromHeader(Name = "Accept-Language")] string? locale = null,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var command = new CreateUserCommand
            {
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                FullName = request.FullName,
                Role = request.Role,
                PhoneNumber = request.PhoneNumber,
                PreferredLanguage = locale ?? DefaultValues.DefaultLanguage
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning($"Failed to create user: {result.Message}");

                // Return 409 Conflict if email already exists
                if (result.Message.Contains("already"))
                {
                    return Conflict(new { error = result.Message });
                }

                return BadRequest(new { error = result.Message });
            }

            return CreatedAtAction(nameof(GetUser), 
                new { id = result.Value?.Id }, 
                result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating user: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An error occurred while creating the user" });
        }
    }

    /// <summary>
    /// Get all active users (paginated).
    /// </summary>
    /// <param name="pageNumber">The page number (default: 1)</param>
    /// <param name="pageSize">The page size (default: 20, max: 100)</param>
    /// <returns>Paginated list of active users</returns>
    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<UserResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetActiveUsers(
        [FromQuery(Name = "pageNumber")] int pageNumber = 1,
        [FromQuery(Name = "pageSize")] int pageSize = DefaultValues.DefaultPageSize,
        [FromHeader(Name = "Accept-Language")] string? locale = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new ListActiveUsersQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                PreferredLanguage = locale
            };

            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Message });
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error listing active users: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An error occurred while retrieving users" });
        }
    }

    /// <summary>
    /// Assign a role to a user.
    /// </summary>
    /// <param name="id">The user ID</param>
    /// <param name="roleRequest">The role to assign</param>
    /// <returns>Updated user details</returns>
    [HttpPost("{id:guid}/roles")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignRole(
        Guid id,
        [FromBody] AssignRoleRequest roleRequest,
        [FromHeader(Name = "Accept-Language")] string? locale = null,
        CancellationToken cancellationToken = default)
    {
        if (roleRequest == null)
        {
            return BadRequest(new { error = "Role assignment request is required" });
        }

        try
        {
            var command = new AssignRoleCommand
            {
                UserId = id,
                NewRole = roleRequest.Role,
                PreferredLanguage = locale
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                if (result.Message.Contains("not found"))
                {
                    return NotFound(new { error = result.Message });
                }

                return BadRequest(new { error = result.Message });
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error assigning role to user {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An error occurred while assigning the role" });
        }
    }

    /// <summary>
    /// Grant a permission to a user.
    /// </summary>
    /// <param name="id">The user ID</param>
    /// <param name="permissionRequest">The permission to grant</param>
    /// <returns>Updated user details</returns>
    [HttpPost("{id:guid}/permissions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GrantPermission(
        Guid id,
        [FromBody] GrantPermissionRequest permissionRequest,
        [FromHeader(Name = "Accept-Language")] string? locale = null,
        CancellationToken cancellationToken = default)
    {
        if (!User.IsInRole("Admin"))
        {
            // Cashier cannot grant permissions they don't have. In fact, for this system let's say only Admins can grant.
            return Forbid();
        }
        if (permissionRequest == null)
        {
            return BadRequest(new { error = "Permission grant request is required" });
        }

        try
        {
            var command = new GrantPermissionCommand
            {
                UserId = id,
                Permission = permissionRequest.Permission,
                PreferredLanguage = locale
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                if (result.Message.Contains("not found"))
                {
                    return NotFound(new { error = result.Message });
                }

                return BadRequest(new { error = result.Message });
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error granting permission to user {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An error occurred while granting the permission" });
        }
    }

    /// <summary>
    /// Update user profile information.
    /// </summary>
    /// <param name="id">The user ID</param>
    /// <param name="request">The updated profile information</param>
    /// <returns>Updated user details</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProfile(
        Guid id,
        [FromBody] UpdateUserProfileRequest request,
        [FromHeader(Name = "Accept-Language")] string? locale = null,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!User.IsInRole("Admin") && currentUserId != id.ToString())
        {
            return Forbid();
        }
        if (request == null)
        {
            return BadRequest(new { error = "Update request is required" });
        }

        try
        {
            var command = new UpdateUserProfileCommand
            {
                UserId = id,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                PreferredLanguage = locale
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                if (result.Message.Contains("not found"))
                {
                    return NotFound(new { error = result.Message });
                }

                return BadRequest(new { error = result.Message });
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating user profile {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An error occurred while updating the profile" });
        }
    }

    /// <summary>
    /// Deactivate a user.
    /// </summary>
    /// <param name="id">The user ID</param>
    /// <returns>Updated user details (IsActive = false)</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeactivateUser(
        Guid id,
        [FromHeader(Name = "Accept-Language")] string? locale = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeactivateUserCommand
            {
                UserId = id,
                PreferredLanguage = locale
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                if (result.Message.Contains("not found"))
                {
                    return NotFound(new { error = result.Message });
                }

                return BadRequest(new { error = result.Message });
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deactivating user {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An error occurred while deactivating the user" });
        }
    }

    /// <summary>
    /// Check if a user has a specific permission.
    /// </summary>
    /// <param name="id">The user ID</param>
    /// <param name="permission">The permission to check</param>
    /// <returns>Boolean indicating if user has the permission</returns>
    [HttpGet("{id:guid}/permissions/{permission}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckPermission(
        Guid id,
        [FromRoute] PermissionType permission,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new CheckUserPermissionQuery
            {
                UserId = id,
                Permission = permission
            };

            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Message });
            }

            return Ok(new { hasPermission = result.Value });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking permission for user {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An error occurred while checking the permission" });
        }
    }    [HttpPost("login")]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] POS.Application.Features.UserManagement.Commands.LoginCommand request)
    {
        var result = await _mediator.Send(request);
        if (result.IsSuccess) return Ok(new { token = result.Value });
        return Unauthorized(new { error = result.Error });
    }
}

/// <summary>
/// Request model for assigning a role to a user.
/// </summary>
public class AssignRoleRequest
{
    public UserRole Role { get; set; }
}

/// <summary>
/// Request model for granting a permission to a user.
/// </summary>
public class GrantPermissionRequest
{
    public PermissionType Permission { get; set; }

}





