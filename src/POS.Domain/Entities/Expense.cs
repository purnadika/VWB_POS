using System;
using POS.Domain.Common;

namespace POS.Domain.Entities;

public class Expense : BaseEntity
{
    public int CategoryId { get; set; }
    public ExpenseCategory? Category { get; set; }
    public decimal Amount { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}
