using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Domain.Aggregates.UserManagement;
using POS.Domain.Interfaces.Repositories;
using POS.Domain.ValueObjects;

namespace POS.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for User aggregate.
/// Handles persistence operations for users with Guid-based IDs.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly POSDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(POSDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error finding user by ID: {ex.Message}");
            throw;
        }
    }

    public async Task<User?> FindByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error finding user by email: {ex.Message}");
            throw;
        }
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var emailVo = new Email(email);
            return await FindByEmailAsync(emailVo, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error finding user by email string: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _context.Users
                .Where(u => u.IsActive)
                .OrderBy(u => u.Email)
                .ToListAsync(cancellationToken);

            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting active users: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<User>> GetInactiveUsersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _context.Users
                .Where(u => !u.IsActive)
                .OrderBy(u => u.Email)
                .ToListAsync(cancellationToken);

            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting inactive users: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await _context.Users
                .AnyAsync(u => u.Email == email, cancellationToken);

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking if email exists: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> EmailExistsAsync(Email email, Guid excludeUserId, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await _context.Users
                .AnyAsync(u => u.Email == email && u.Id != excludeUserId, cancellationToken);

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking if email exists (excluding user): {ex.Message}");
            throw;
        }
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding user: {ex.Message}");
            throw;
        }
    }

    public void Update(User user)
    {
        try
        {
            _context.Users.Update(user);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating user: {ex.Message}");
            throw;
        }
    }
}
