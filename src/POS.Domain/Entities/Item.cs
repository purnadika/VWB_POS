using System.Collections.Generic;
using POS.Domain.Common;

namespace POS.Domain.Entities;

public class Item : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public ItemCategory? Category { get; set; }
    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public string ItemNumber { get; set; } = string.Empty; // SKU / Barcode text
    public string Description { get; set; } = string.Empty;
    public decimal CostPrice { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ReorderLevel { get; set; } = 0;
    public decimal ReceivingQuantity { get; set; } = 1;
    public bool IsSerialized { get; set; } = false;
    public bool AllowAltDescription { get; set; } = false;

    public int? TaxCategoryId { get; set; }
    public TaxCategory? TaxCategory { get; set; }

    public List<ItemQuantity> ItemQuantities { get; set; } = new();
    public List<ItemAttributeLink> AttributeLinks { get; set; } = new();
}

