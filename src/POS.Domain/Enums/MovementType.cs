namespace POS.Domain.Enums;

public enum MovementType
{
    StockIn,       // Receiving / Purchase Order
    StockOut,      // Sale
    Adjustment,    // Manual Correction
    Reorder,       // Auto Alert Reorder
    Return         // Sale Return / Refund
}
