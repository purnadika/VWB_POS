using POS.Domain.Common;
using POS.Domain.ValueObjects;

namespace POS.Domain.Entities;

public abstract class Person : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Address Address { get; set; } = new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
    public string Comments { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}".Trim();
}
