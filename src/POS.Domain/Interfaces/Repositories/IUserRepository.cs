using POS.Domain.Aggregates.UserManagement;
using POS.Domain.ValueObjects;

namespace POS.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for User aggregate.
/// Abstracts data access layer from domain logic.
/// Handles Guid-based IDs unlike the generic Repository<T> which uses int IDs.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Finds a user by ID.
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a user by email address.
    /// </summary>
    Task<User?> FindByEmailAsync(Email email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a user by email string.
    /// </summary>
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active users.
    /// </summary>
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all inactive users.
    /// </summary>
    Task<IEnumerable<User>> GetInactiveUsersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an email is already in use.
    /// </summary>
    Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an email is already in use (excluding a specific user).
    /// </summary>
    Task<bool> EmailExistsAsync(Email email, Guid excludeUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a user to the repository.
    /// </summary>
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    void Update(User user);

    /// <summary>
    /// Gets the unit of work for this repository.
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
}
