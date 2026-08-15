using POS.Domain.Common;

namespace POS.Domain.Entities;

public class ItemQuantity : BaseEntity
{
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    public int LocationId { get; set; }
    public StockLocation? Location { get; set; }
    public decimal Quantity { get; set; } = 0;
}
