using System.ComponentModel.DataAnnotations;

namespace POS.Domain.Entities;

public class AppConfig
{
    [Key]
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
