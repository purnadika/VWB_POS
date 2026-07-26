using System;

namespace POS.Domain.ValueObjects;

/// <summary>
/// PasswordHash value object stores hashed passwords securely.
/// Never stores plain text passwords.
/// </summary>
public class PasswordHash : ValueObject
{
    public string Hash { get; }

    /// <summary>
    /// Password must be provided already hashed (using BCrypt or similar).
    /// </summary>
    public PasswordHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Password hash cannot be empty", nameof(hash));

        Hash = hash;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Hash;
    }

    public override string ToString() => "***";
}
