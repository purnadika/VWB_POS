using System;
using System.Threading;
using System.Threading.Tasks;

namespace POS.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
