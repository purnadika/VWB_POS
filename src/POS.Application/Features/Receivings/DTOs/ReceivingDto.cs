using System;
using POS.Domain.Common;

namespace POS.Application.Features.Receivings.DTOs;

public class ReceivingDto
{
    public int Id { get; set; }
    public int? SupplierId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime ReceivingTime { get; set; } = DateTime.UtcNow;
    public string Comment { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string PaymentType { get; set; } = string.Empty;
    public decimal Total { get; set; }
}
