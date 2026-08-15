using System.Threading;
using System.Threading.Tasks;
using POS.Domain.Entities;

namespace POS.Domain.Interfaces.Repositories;

public interface IAuthLogRepository
{
    Task AddAsync(AuthLog authLog, CancellationToken cancellationToken = default);
}
