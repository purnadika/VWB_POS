namespace POS.Domain.Enums;

public enum SaleStatus
{
    Completed,
    Refunded,
    Suspended, // Draft checkouts / saved carts
    Cancelled
}
