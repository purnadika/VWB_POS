using POS.Domain.Common;

namespace POS.Application.Features.GiftCards.DTOs;

public class GiftcardDto
{
    public int Id { get; set; }
    public string GiftcardNumber { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public int? CustomerId { get; set; }
}
