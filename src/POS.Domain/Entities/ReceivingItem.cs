using POS.Domain.Common;

namespace POS.Domain.Entities;

public class ReceivingItem : BaseEntity
{
    public int ReceivingId { get; set; }
    public Receiving? Receiving { get; set; }
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; } // Unit cost price from supplier
    public decimal DiscountPercent { get; set; } = 0.00m;
    public decimal LineTotal => (Quantity * UnitPrice) * (1 - (DiscountPercent / 100m));
}
