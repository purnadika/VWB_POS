using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Infrastructure.Persistence.Repositories;

public class ItemRepository : Repository<Item>, IItemRepository
{
    public ItemRepository(POSDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Items
            .Include(i => i.Supplier)
            .Include(i => i.TaxCategory)
            .Include(i => i.ItemQuantities).ThenInclude(iq => iq.Location)
            .Include(i => i.AttributeLinks).ThenInclude(al => al.Attribute)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<Item?> GetByItemNumberAsync(string itemNumber, CancellationToken cancellationToken = default)
    {
        return await DbContext.Items
            .Include(i => i.Supplier)
            .Include(i => i.TaxCategory)
            .Include(i => i.ItemQuantities).ThenInclude(iq => iq.Location)
            .Include(i => i.AttributeLinks).ThenInclude(al => al.Attribute)
            .FirstOrDefaultAsync(i => i.ItemNumber == itemNumber && !i.Deleted, cancellationToken);
    }

    public async Task<decimal> GetStockLevelAsync(int itemId, int locationId, CancellationToken cancellationToken = default)
    {
        var itemQty = await DbContext.ItemQuantities
            .FirstOrDefaultAsync(iq => iq.ItemId == itemId && iq.LocationId == locationId, cancellationToken);
        return itemQty?.Quantity ?? 0m;
    }

    public async Task UpdateStockLevelAsync(int itemId, int locationId, decimal quantityChange, CancellationToken cancellationToken = default)
    {
        var itemQty = await DbContext.ItemQuantities
            .FirstOrDefaultAsync(iq => iq.ItemId == itemId && iq.LocationId == locationId, cancellationToken);

        if (itemQty != null)
        {
            itemQty.Quantity += quantityChange;
        }
        else
        {
            itemQty = new ItemQuantity
            {
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantityChange
            };
            await DbContext.ItemQuantities.AddAsync(itemQty, cancellationToken);
        }

        // Add inventory transaction log
        var transaction = new InventoryTransaction
        {
            ItemId = itemId,
            LocationId = locationId,
            Quantity = quantityChange,
            Comment = "Stock updated via transaction.",
            TransDate = DateTime.UtcNow,
            EmployeeId = 1 // Default Admin
        };
        await DbContext.InventoryTransactions.AddAsync(transaction, cancellationToken);
    }
}
