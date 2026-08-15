using System;
using System.Collections.Generic;
using POS.Domain.Common;
using POS.Domain.Enums;

namespace POS.Domain.Entities;

public class Sale : BaseEntity
{
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public DateTime SaleTime { get; set; } = DateTime.UtcNow;
    public string Comment { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public SaleStatus Status { get; set; } = SaleStatus.Completed;
    public int? DinnerTableId { get; set; }
    public DinnerTable? DinnerTable { get; set; }

    public List<SaleItem> SaleItems { get; set; } = new();
    public List<SalePayment> Payments { get; set; } = new();
    public List<SaleTax> SaleTaxes { get; set; } = new();

    public decimal SubTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal Total { get; set; }
}
