namespace POS.Application.Constants;

/// <summary>
/// Default configuration values for the application.
/// NO MAGIC NUMBERS - all defaults are constants.
/// </summary>
public static class DefaultValues
{
    // Localization
    public const string DefaultLanguage = "en";
    public const string DefaultCurrency = "USD";

    // Pagination
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    // Password Requirements
    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 128;

    // Full Name
    public const int MinFullNameLength = 2;
    public const int MaxFullNameLength = 100;

    // Email
    public const int MaxEmailLength = 254;

    // Phone
    public const int MinPhoneLength = 10;
    public const int MaxPhoneLength = 20;

    // Timeouts
    public const int SessionTimeoutMinutes = 30;
    public const int PasswordResetTokenExpiryMinutes = 60;
}
