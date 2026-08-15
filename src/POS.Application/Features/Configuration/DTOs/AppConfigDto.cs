using POS.Domain.Common;

namespace POS.Application.Features.Configuration.DTOs;

public class AppConfigDto
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
