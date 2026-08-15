using POS.Domain.Common;

namespace POS.Domain.Entities;

public class TaxRate : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; } // Percentage rate, e.g. 8.25m for 8.25%
    public int TaxCategoryId { get; set; }
    public TaxCategory? TaxCategory { get; set; }

}

