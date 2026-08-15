using System;
using System.Collections.Generic;
using POS.Domain.Common;

namespace POS.Domain.Entities;

public class Receiving : BaseEntity
{
    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public DateTime ReceivingTime { get; set; } = DateTime.UtcNow;
    public string Comment { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty; // Supplier invoice or reference number
    public string PaymentType { get; set; } = string.Empty; // Cash, credit etc.
    public decimal Total { get; set; }

    public List<ReceivingItem> ReceivingItems { get; set; } = new();
}
