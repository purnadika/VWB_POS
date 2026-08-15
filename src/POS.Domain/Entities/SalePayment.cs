using POS.Domain.Common;
using POS.Domain.Enums;

namespace POS.Domain.Entities;

public class SalePayment : BaseEntity
{
    public int SaleId { get; set; }
    public Sale? Sale { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
    public decimal Amount { get; set; }
}
