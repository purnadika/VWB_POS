using System;
using POS.Domain.Common;

namespace POS.Domain.Entities;

public class AuthLog : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string Details { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
