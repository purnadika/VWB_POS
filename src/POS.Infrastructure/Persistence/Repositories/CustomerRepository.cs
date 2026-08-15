using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Infrastructure.Persistence.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(POSDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Customer?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
    {
        return await DbContext.Customers
            .FirstOrDefaultAsync(c => c.AccountNumber == accountNumber && !c.Deleted, cancellationToken);
    }
}
