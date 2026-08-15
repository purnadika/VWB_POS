using System;
using POS.Domain.Common;

namespace POS.Application.Features.Expenses.DTOs;

public class ExpenseDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}
