using POS.Domain.Common;

namespace POS.Application.Features.ItemKits.DTOs;

public class ItemKitDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ItemKitNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? UnitPrice { get; set; }
    public decimal? CostPrice { get; set; }
}
