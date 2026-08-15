namespace POS.Domain.Entities;

public class Supplier : Person
{
    public string CompanyName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AgencyName { get; set; } = string.Empty;

}

