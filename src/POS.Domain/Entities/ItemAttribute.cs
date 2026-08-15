using POS.Domain.Common;
using POS.Domain.Enums;

namespace POS.Domain.Entities;

public class ItemAttribute : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public AttributeType Type { get; set; } = AttributeType.Text;

}

