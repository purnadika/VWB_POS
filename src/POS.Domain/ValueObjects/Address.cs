using System;

namespace POS.Domain.ValueObjects;

public class Address : IEquatable<Address>
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string Zip { get; }
    public string Country { get; }

    public Address(string street, string city, string state, string zip, string country)
    {
        Street = street ?? string.Empty;
        City = city ?? string.Empty;
        State = state ?? string.Empty;
        Zip = zip ?? string.Empty;
        Country = country ?? string.Empty;
    }

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        return Street == other.Street &&
               City == other.City &&
               State == other.State &&
               Zip == other.Zip &&
               Country == other.Country;
    }

    public override bool Equals(object? obj) => Equals(obj as Address);

    public override int GetHashCode() => HashCode.Combine(Street, City, State, Zip, Country);

    public static bool operator ==(Address? left, Address? right) => Equals(left, right);
    public static bool operator !=(Address? left, Address? right) => !Equals(left, right);
}
