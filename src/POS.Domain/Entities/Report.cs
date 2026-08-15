using System;  
using POS.Domain.Common;  
namespace POS.Domain.Entities;  
  
public class Report : BaseEntity  
{  
    public string Name { get; set; } = string.Empty;  
    public string Description { get; set; } = string.Empty;  
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;  
    public string DataJson { get; set; } = string.Empty;  
    public int GeneratedById { get; set; }  
    public Employee? GeneratedBy { get; set; }  
} 
