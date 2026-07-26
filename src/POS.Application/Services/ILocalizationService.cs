namespace POS.Application.Services;

/// <summary>
/// Service for retrieving localized strings based on language and localization key.
/// NO MAGIC STRINGS - all user-facing text goes through this service.
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// Gets a localized string by key.
    /// </summary>
    /// <param name="key">The localization key (e.g., "user.created.successfully")</param>
    /// <param name="locale">The language code (e.g., "en", "fr"). Defaults to system default.</param>
    /// <returns>The localized string, or the key if translation not found</returns>
    string GetString(string key, string? locale = null);

    /// <summary>
    /// Gets a localized string with parameter substitution.
    /// </summary>
    /// <param name="key">The localization key</param>
    /// <param name="parameters">Dictionary of parameters to substitute in the string</param>
    /// <param name="locale">The language code</param>
    /// <returns>The localized string with parameters substituted</returns>
    string GetString(string key, Dictionary<string, object>? parameters = null, string? locale = null);

    /// <summary>
    /// Gets all available language codes.
    /// </summary>
    IEnumerable<string> GetAvailableLanguages();

    /// <summary>
    /// Gets the default language code for the application.
    /// </summary>
    string GetDefaultLanguage();

    /// <summary>
    /// Checks if a language is available.
    /// </summary>
    bool IsLanguageAvailable(string locale);
}
