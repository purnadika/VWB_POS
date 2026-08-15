using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using POS.Domain.Entities;

namespace POS.Domain.Interfaces.Repositories;

public interface ISaleRepository : IRepository<Sale>
{
    Task<Sale?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Sale>> GetSuspendedSalesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Sale>> GetSalesRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
