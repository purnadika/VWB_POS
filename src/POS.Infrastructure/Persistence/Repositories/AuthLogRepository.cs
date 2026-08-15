using System.Threading;
using System.Threading.Tasks;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Infrastructure.Persistence;

namespace POS.Infrastructure.Persistence.Repositories;

public class AuthLogRepository : IAuthLogRepository
{
    private readonly POSDbContext _context;

    public AuthLogRepository(POSDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuthLog authLog, CancellationToken cancellationToken = default)
    {
        await _context.AuthLogs.AddAsync(authLog, cancellationToken);
    }
}
