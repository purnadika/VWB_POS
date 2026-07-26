using Microsoft.Extensions.Logging;
using POS.Application.Constants;
using POS.Application.Services;

namespace POS.Infrastructure.Services;

/// <summary>
/// Implementation of currency service.
/// Manages ISO 4217 currency codes and exchange rates.
/// </summary>
public class CurrencyService : ICurrencyService
{
    private readonly ILogger<CurrencyService> _logger;
    private readonly Dictionary<string, string> _currencies;
    private readonly string _defaultCurrency;

    public CurrencyService(ILogger<CurrencyService> logger)
    {
        _logger = logger;
        _defaultCurrency = DefaultValues.DefaultCurrency;

        // Initialize supported currencies (ISO 4217)
        _currencies = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Major currencies
            { "USD", "United States Dollar" },
            { "EUR", "Euro" },
            { "GBP", "British Pound" },
            { "JPY", "Japanese Yen" },
            { "CHF", "Swiss Franc" },
            { "CAD", "Canadian Dollar" },
            { "AUD", "Australian Dollar" },

            // Common currencies
            { "INR", "Indian Rupee" },
            { "CNY", "Chinese Yuan" },
            { "MXN", "Mexican Peso" },
            { "SGD", "Singapore Dollar" },
            { "HKD", "Hong Kong Dollar" },
            { "NZD", "New Zealand Dollar" },
            { "SEK", "Swedish Krona" },
            { "NOK", "Norwegian Krone" },
            { "DKK", "Danish Krone" },

            // Add more currencies as needed
        };
    }

    public IEnumerable<string> GetAvailableCurrencies()
    {
        return _currencies.Keys.OrderBy(x => x);
    }

    public (string Code, string Name)? GetCurrency(string code)
    {
        if (_currencies.TryGetValue(code, out var name))
        {
            return (code.ToUpperInvariant(), name);
        }

        return null;
    }

    public bool IsCurrencyValid(string code)
    {
        return _currencies.ContainsKey(code);
    }

    public string GetDefaultCurrency() => _defaultCurrency;

    public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
    {
        // Validate currencies
        if (!IsCurrencyValid(fromCurrency) || !IsCurrencyValid(toCurrency))
        {
            _logger.LogWarning($"Invalid currency in exchange rate request: {fromCurrency} -> {toCurrency}");
            return 1.0m;
        }

        // In production, this would call an external API (e.g., OpenExchangeRates, IEX Cloud)
        // For MVP, return 1.0 if same currency, else a placeholder
        if (fromCurrency.Equals(toCurrency, StringComparison.OrdinalIgnoreCase))
        {
            return 1.0m;
        }

        // Placeholder: In production, fetch from external service
        // For now, return a dummy rate
        _logger.LogDebug($"Exchange rate requested: {fromCurrency} -> {toCurrency}. Using placeholder rate.");
        return 1.0m;
    }

    public async Task<(decimal Amount, string Currency)> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
    {
        if (fromCurrency.Equals(toCurrency, StringComparison.OrdinalIgnoreCase))
        {
            return (amount, toCurrency);
        }

        var rate = await GetExchangeRateAsync(fromCurrency, toCurrency);
        var convertedAmount = amount * rate;

        return (convertedAmount, toCurrency);
    }
}
