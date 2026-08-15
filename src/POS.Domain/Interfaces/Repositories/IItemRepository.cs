using System.Threading;
using System.Threading.Tasks;
using POS.Domain.Entities;

namespace POS.Domain.Interfaces.Repositories;

public interface IItemRepository : IRepository<Item>
{
    Task<Item?> GetByItemNumberAsync(string itemNumber, CancellationToken cancellationToken = default);
    Task<decimal> GetStockLevelAsync(int itemId, int locationId, CancellationToken cancellationToken = default);
    Task UpdateStockLevelAsync(int itemId, int locationId, decimal quantityChange, CancellationToken cancellationToken = default);
}
