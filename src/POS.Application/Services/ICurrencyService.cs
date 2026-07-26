namespace POS.Application.Services;

/// <summary>
/// Service for handling multi-currency operations.
/// Manages currency codes, exchange rates, and conversions.
/// </summary>
public interface ICurrencyService
{
    /// <summary>
    /// Gets all available currency codes (ISO 4217).
    /// </summary>
    IEnumerable<string> GetAvailableCurrencies();

    /// <summary>
    /// Gets currency details by code.
    /// </summary>
    /// <param name="code">ISO 4217 currency code (e.g., "USD", "EUR")</param>
    /// <returns>Currency code and name, or null if not found</returns>
    (string Code, string Name)? GetCurrency(string code);

    /// <summary>
    /// Checks if a currency code is valid.
    /// </summary>
    bool IsCurrencyValid(string code);

    /// <summary>
    /// Gets the default currency for the system.
    /// </summary>
    string GetDefaultCurrency();

    /// <summary>
    /// Gets the exchange rate between two currencies.
    /// For now, returns 1.0 (rates would be fetched from external service in production).
    /// </summary>
    Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency);

    /// <summary>
    /// Converts an amount from one currency to another.
    /// </summary>
    Task<(decimal Amount, string Currency)> ConvertAsync(decimal amount, string fromCurrency, string toCurrency);
}
