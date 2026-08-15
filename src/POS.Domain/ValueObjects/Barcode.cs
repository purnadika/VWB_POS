using System;
using POS.Domain.Enums;

namespace POS.Domain.ValueObjects;

public class Barcode : IEquatable<Barcode>
{
    public string Value { get; }
    public BarcodeType Type { get; }

    public Barcode(string value, BarcodeType type)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Barcode value cannot be empty.", nameof(value));

        Value = value;
        Type = type;
    }

    public bool Equals(Barcode? other)
    {
        if (other is null) return false;
        return Value == other.Value && Type == other.Type;
    }

    public override bool Equals(object? obj) => Equals(obj as Barcode);

    public override int GetHashCode() => HashCode.Combine(Value, Type);

    public static bool operator ==(Barcode? left, Barcode? right) => Equals(left, right);
    public static bool operator !=(Barcode? left, Barcode? right) => !Equals(left, right);
}
