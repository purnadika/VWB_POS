using POS.Domain.Common;

namespace POS.Domain.Entities;

public class SaleItem : BaseEntity
{
    public int SaleId { get; set; }
    public Sale? Sale { get; set; }
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    public string Description { get; set; } = string.Empty; // Description override
    public string SerialNumber { get; set; } = string.Empty; // Serial number for serialized items
    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal CostPrice { get; set; }
    public decimal DiscountPercent { get; set; } = 0.00m; // E.g., 10.00 for 10%
    public decimal LineTotal => (Quantity * UnitPrice) * (1 - (DiscountPercent / 100m));
}
