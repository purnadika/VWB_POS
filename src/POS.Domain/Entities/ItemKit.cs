using System.Collections.Generic;
using POS.Domain.Common;

namespace POS.Domain.Entities;

public class ItemKit : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ItemKitNumber { get; set; } = string.Empty; // SKU / Barcode text
    public string Description { get; set; } = string.Empty;
    public decimal? UnitPrice { get; set; } // Null if dynamically calculated from components
    public decimal? CostPrice { get; set; } // Null if dynamically calculated from components

    public List<ItemKitItem> KitItems { get; set; } = new();
}

