using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Domain.Enums;
using POS.Domain.Interfaces.Repositories;

namespace POS.Infrastructure.Persistence.Repositories;

public class SaleRepository : Repository<Sale>, ISaleRepository
{
    public SaleRepository(POSDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.SaleItems).ThenInclude(si => si.Item)
            .Include(s => s.Payments)
            .Include(s => s.SaleTaxes)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Sale?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default)
    {
        return await DbContext.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.SaleItems).ThenInclude(si => si.Item)
            .Include(s => s.Payments)
            .Include(s => s.SaleTaxes)
            .FirstOrDefaultAsync(s => s.InvoiceNumber == invoiceNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<Sale>> GetSuspendedSalesAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Where(s => s.Status == SaleStatus.Suspended)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Sale>> GetSalesRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await DbContext.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.SaleItems)
            .Where(s => s.SaleTime >= startDate && s.SaleTime <= endDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Sale>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .ToListAsync(cancellationToken);
    }
}
