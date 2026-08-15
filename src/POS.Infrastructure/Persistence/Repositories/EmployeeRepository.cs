using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(POSDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Employee?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await DbContext.Employees
            .FirstOrDefaultAsync(e => e.Username == username && !e.Deleted, cancellationToken);
    }
}
