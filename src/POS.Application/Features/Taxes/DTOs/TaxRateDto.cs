using POS.Domain.Common;

namespace POS.Application.Features.Taxes.DTOs;

public class TaxRateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int TaxCategoryId { get; set; }
}
