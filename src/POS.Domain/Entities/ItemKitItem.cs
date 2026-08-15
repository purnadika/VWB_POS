using POS.Domain.Common;

namespace POS.Domain.Entities;

public class ItemKitItem : BaseEntity
{
    public int ItemKitId { get; set; }
    public ItemKit? ItemKit { get; set; }
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    public decimal Quantity { get; set; } = 1;
}
