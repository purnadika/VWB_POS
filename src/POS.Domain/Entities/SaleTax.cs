using POS.Domain.Common;

namespace POS.Domain.Entities;

public class SaleTax : BaseEntity
{
    public int SaleId { get; set; }
    public Sale? Sale { get; set; }
    public string TaxName { get; set; } = string.Empty;
    public decimal Rate { get; set; } // Percentage rate, e.g. 8.25 for 8.25%
    public decimal Amount { get; set; }
}
