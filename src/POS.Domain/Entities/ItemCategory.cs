using POS.Domain.Common;

namespace POS.Domain.Entities;

public class ItemCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
