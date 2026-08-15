using POS.Domain.Common;

namespace POS.Domain.Entities;

public class Giftcard : BaseEntity
{
    public string GiftcardNumber { get; set; } = string.Empty;
    public decimal Value { get; set; }

    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
}

