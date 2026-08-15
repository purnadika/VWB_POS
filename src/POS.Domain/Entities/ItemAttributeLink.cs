using POS.Domain.Common;

namespace POS.Domain.Entities;

public class ItemAttributeLink : BaseEntity
{
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    public int AttributeId { get; set; }
    public ItemAttribute? Attribute { get; set; }
    public string Value { get; set; } = string.Empty; // Serialized value depending on AttributeType
}
