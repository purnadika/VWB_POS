using System;
using System.Text.RegularExpressions;

namespace POS.Domain.ValueObjects;

/// <summary>
/// Email value object ensures email addresses are always valid and immutable.
/// Implements value object pattern with equality based on email value.
/// </summary>
public class Email : ValueObject
{
    private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty", nameof(value));

        if (!IsValidEmail(value))
            throw new ArgumentException($"Invalid email format: {value}", nameof(value));

        Value = value.ToLower().Trim();
    }

    private static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, EmailPattern, RegexOptions.IgnoreCase);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
    public static explicit operator Email(string value) => new(value);
}
