using System;
using POS.Domain.Common;
using POS.Domain.Enums;

namespace POS.Domain.Entities;

public class InventoryTransaction : BaseEntity
{
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public DateTime TransDate { get; set; } = DateTime.UtcNow;
    public string Comment { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public StockLocation? Location { get; set; }
    public decimal Quantity { get; set; } // Change in quantity (+/-)
    public MovementType MovementType { get; set; } = MovementType.StockIn;
}
