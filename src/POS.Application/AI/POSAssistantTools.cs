using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.AI;

public class POSAssistantTools
{
    private readonly IItemRepository _itemRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IRepository<Supplier> _supplierRepository;

    public POSAssistantTools(
        IItemRepository itemRepository,
        ISaleRepository saleRepository,
        IRepository<Supplier> supplierRepository)
    {
        _itemRepository = itemRepository;
        _saleRepository = saleRepository;
        _supplierRepository = supplierRepository;
    }

    [Description("Gets the current stock level and details of an item by name or SKU.")]
    public async Task<string> GetInventoryStatus(
        [Description("The name or SKU of the item to search for.")] string searchToken,
        CancellationToken cancellationToken)
    {
        var items = await _itemRepository.GetAllAsync(cancellationToken);
        var matchedItem = items.FirstOrDefault(i =>
            i.Name.Contains(searchToken, StringComparison.OrdinalIgnoreCase) ||
            i.ItemNumber.Equals(searchToken, StringComparison.OrdinalIgnoreCase));

        if (matchedItem == null)
            return $"No items matching '{searchToken}' were found in the catalog.";

        var stockLevel = matchedItem.ItemQuantities.Sum(iq => iq.Quantity);

        return $"Item: {matchedItem.Name} (SKU: {matchedItem.ItemNumber})\n" +
               $"Category: {matchedItem.Category}\n" +
               $"Unit Price: ${matchedItem.UnitPrice}\n" +
               $"Current Stock: {stockLevel} units\n" +
               $"Reorder Level: {matchedItem.ReorderLevel} units";
    }

    [Description("Summarizes total sales, transactions, and average check size over a given number of past days.")]
    public async Task<string> GetSalesSummary(
        [Description("The number of past days to analyze. Default is 7.")] int days,
        CancellationToken cancellationToken)
    {
        var startDate = DateTime.UtcNow.Date.AddDays(-days);
        var endDate = DateTime.UtcNow;

        var sales = await _saleRepository.GetSalesRangeAsync(startDate, endDate, cancellationToken);

        if (!sales.Any())
            return $"No sales recorded in the past {days} days.";

        var totalRevenue = sales.Sum(s => s.Total);
        var transactionCount = sales.Count;
        var avgCheck = transactionCount > 0 ? totalRevenue / transactionCount : 0m;

        return $"Sales Summary (Past {days} Days):\n" +
               $"- Total Revenue: ${totalRevenue:N2}\n" +
               $"- Total Transactions: {transactionCount}\n" +
               $"- Average Transaction Amount: ${avgCheck:N2}";
    }

    [Description("Drafts a recommended purchase order for restocking items from a specific supplier.")]
    public async Task<string> DraftPurchaseOrder(
        [Description("The ID of the supplier.")] int supplierId,
        CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(supplierId, cancellationToken);
        if (supplier == null)
            return $"Supplier with ID {supplierId} not found.";

        var items = await _itemRepository.GetAllAsync(cancellationToken);
        var lowStockSupplierItems = items
            .Where(i => i.SupplierId == supplierId && !i.Deleted)
            .Select(i => new
            {
                Item = i,
                Stock = i.ItemQuantities.Sum(q => q.Quantity)
            })
            .Where(x => x.Stock <= x.Item.ReorderLevel)
            .ToList();

        if (!lowStockSupplierItems.Any())
            return $"All items from supplier '{supplier.CompanyName}' have healthy stock levels. No restocking is currently needed.";

        var poText = $"DRAFT PURCHASE ORDER - {supplier.CompanyName}\n";
        poText += $"Date: {DateTime.UtcNow:d}\n";
        poText += "--------------------------------------------------------\n";
        poText += "SKU       | Item Name            | Stock | Reorder Level | Suggested Order Qty\n";
        poText += "--------------------------------------------------------\n";

        foreach (var entry in lowStockSupplierItems)
        {
            var suggestedQty = entry.Item.ReceivingQuantity * 2; // Simple heuristic: order twice the receiving quantity
            poText += $"{entry.Item.ItemNumber,-9} | {entry.Item.Name,-20} | {entry.Stock,-5:N0} | {entry.Item.ReorderLevel,-13:N0} | {suggestedQty:N0}\n";
        }

        return poText;
    }
}
