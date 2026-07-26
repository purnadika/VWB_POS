using POS.Domain.Enums;

namespace POS.Domain.Aggregates.UserManagement.Events;

/// <summary>
/// Base class for domain events.
/// Domain events are published to communicate state changes to other parts of the system.
/// </summary>
public abstract class DomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <summary>
/// Domain event raised when a new user is created.
/// Used for audit trail and AI agent reasoning.
/// </summary>
public class UserCreatedDomainEvent : DomainEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

/// <summary>
/// Domain event raised when a user's role is changed.
/// </summary>
public class UserRoleChangedDomainEvent : DomainEvent
{
    public Guid UserId { get; set; }
    public UserRole OldRole { get; set; }
    public UserRole NewRole { get; set; }
    public DateTime ChangedAt { get; set; }
    public string ChangedBy { get; set; } = string.Empty;
}

/// <summary>
/// Domain event raised when permissions are granted to a user.
/// </summary>
public class UserPermissionGrantedDomainEvent : DomainEvent
{
    public Guid UserId { get; set; }
    public PermissionType Permission { get; set; }
    public DateTime GrantedAt { get; set; }
    public string GrantedBy { get; set; } = string.Empty;
}

/// <summary>
/// Domain event raised when a user is deactivated.
/// </summary>
public class UserDeactivatedDomainEvent : DomainEvent
{
    public Guid UserId { get; set; }
    public DateTime DeactivatedAt { get; set; }
    public string DeactivatedBy { get; set; } = string.Empty;
}
