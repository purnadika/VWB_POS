using System;  
using POS.Domain.Common;  
namespace POS.Domain.Entities;  
  
public class Message : BaseEntity  
{  
    public int SenderId { get; set; }  
    public Employee? Sender { get; set; }  
    public int ReceiverId { get; set; }  
    public Employee? Receiver { get; set; }  
    public string Subject { get; set; } = string.Empty;  
    public string Body { get; set; } = string.Empty;  
    public DateTime SentAt { get; set; } = DateTime.UtcNow;  
} 
