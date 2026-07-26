using System.Text.Json;
using Microsoft.Extensions.Logging;
using POS.Application.Constants;
using POS.Application.Services;

namespace POS.Infrastructure.Services;

/// <summary>
/// Implementation of localization service using JSON files.
/// Loads translations from i18n/[language].json files.
/// </summary>
public class LocalizationService : ILocalizationService
{
    private readonly ILogger<LocalizationService> _logger;
    private readonly string _resourcePath;
    private readonly string _defaultLanguage;
    private Dictionary<string, Dictionary<string, string>> _translations = new();
    private readonly object _lockObject = new();

    public LocalizationService(ILogger<LocalizationService> logger, string resourcePath = "Resources/i18n")
    {
        _logger = logger;
        _resourcePath = resourcePath;
        _defaultLanguage = DefaultValues.DefaultLanguage;
        LoadTranslations();
    }

    public string GetString(string key, string? locale = null)
    {
        return GetString(key, null, locale);
    }

    public string GetString(string key, Dictionary<string, object>? parameters = null, string? locale = null)
    {
        locale ??= _defaultLanguage;

        try
        {
            lock (_lockObject)
            {
                if (!_translations.TryGetValue(locale, out var translations))
                {
                    _logger.LogWarning($"Language '{locale}' not found, falling back to '{_defaultLanguage}'");
                    locale = _defaultLanguage;
                    if (!_translations.TryGetValue(locale, out translations))
                    {
                        return key; // Fallback to key if default language not found
                    }
                }

                if (!translations.TryGetValue(key, out var value))
                {
                    _logger.LogWarning($"Translation key '{key}' not found for language '{locale}'");
                    return key; // Return key if translation not found
                }

                // Replace parameters if provided
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        value = value.Replace($"{{{param.Key}}}", param.Value?.ToString() ?? "");
                    }
                }

                return value;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving translation for key '{key}': {ex.Message}");
            return key;
        }
    }

    public IEnumerable<string> GetAvailableLanguages()
    {
        lock (_lockObject)
        {
            return _translations.Keys.ToList();
        }
    }

    public string GetDefaultLanguage() => _defaultLanguage;

    public bool IsLanguageAvailable(string locale)
    {
        lock (_lockObject)
        {
            return _translations.ContainsKey(locale);
        }
    }

    private void LoadTranslations()
    {
        try
        {
            var languageFiles = GetLanguageFiles();

            foreach (var filePath in languageFiles)
            {
                var language = Path.GetFileNameWithoutExtension(filePath);
                var json = File.ReadAllText(filePath);
                var flatDictionary = FlattenJson(JsonSerializer.Deserialize<JsonElement>(json));

                lock (_lockObject)
                {
                    _translations[language] = flatDictionary;
                }

                _logger.LogInformation($"Loaded translations for language '{language}' from {filePath}");
            }

            if (!_translations.ContainsKey(_defaultLanguage))
            {
                _logger.LogWarning($"Default language '{_defaultLanguage}' not found in translations");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading translations: {ex.Message}");
        }
    }

    private List<string> GetLanguageFiles()
    {
        var files = new List<string>();

        try
        {
            // Try multiple possible paths
            var possiblePaths = new[]
            {
                _resourcePath,
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _resourcePath),
                Path.Combine(Directory.GetCurrentDirectory(), _resourcePath)
            };

            foreach (var path in possiblePaths)
            {
                if (Directory.Exists(path))
                {
                    files.AddRange(Directory.GetFiles(path, "*.json", SearchOption.TopDirectoryOnly));
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error finding language files: {ex.Message}");
        }

        return files;
    }

    private Dictionary<string, string> FlattenJson(JsonElement element, string prefix = "")
    {
        var result = new Dictionary<string, string>();

        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                var key = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";

                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    foreach (var item in FlattenJson(property.Value, key))
                    {
                        result[item.Key] = item.Value;
                    }
                }
                else if (property.Value.ValueKind == JsonValueKind.String)
                {
                    result[key] = property.Value.GetString() ?? "";
                }
            }
        }

        return result;
    }
}
