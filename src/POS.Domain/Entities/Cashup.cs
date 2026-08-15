using System;
using POS.Domain.Common;

namespace POS.Domain.Entities;

public class Cashup : BaseEntity
{
    public DateTime OpenDate { get; set; } = DateTime.UtcNow;
    public DateTime? CloseDate { get; set; }
    public int OpenEmployeeId { get; set; }
    public Employee? OpenEmployee { get; set; }
    public int? CloseEmployeeId { get; set; }
    public Employee? CloseEmployee { get; set; }
    public decimal OpenAmount { get; set; }
    public decimal CloseAmount { get; set; }
    public decimal ClosedAmountDue { get; set; }
    public decimal ClosedAmountReceived { get; set; }
    public decimal ClosedAmountDifference { get; set; }
    public string Notes { get; set; } = string.Empty;
}
