namespace POS.Domain.Entities;

public class Customer : Person
{
    public string CompanyName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public bool Taxable { get; set; } = true;
    public decimal DiscountPercent { get; set; } = 0.00m;
    public int RewardPoints { get; set; } = 0;

}

