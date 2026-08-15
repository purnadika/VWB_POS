using POS.Domain.Common;

namespace POS.Application.Features.Suppliers.DTOs;

public class SupplierDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AgencyName { get; set; } = string.Empty;
}
