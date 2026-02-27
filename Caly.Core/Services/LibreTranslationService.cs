using System;
using System.Threading;
using System.Threading.Tasks;
using Caly.Core.Services.Interfaces;
using LibreTranslate.Net;

namespace Caly.Core.Services;

internal sealed class LibreTranslateService : ITranslationService
{
    private readonly LibreTranslate.Net.LibreTranslate _client;
    private readonly LanguageCode _targetLanguage;

    public LibreTranslateService(ISettingsService settingsService)
    {
        var settings = settingsService.GetSettings();
        _client = new LibreTranslate.Net.LibreTranslate(
            (settings.LibreTranslateUrl ?? "https://libretranslate.com").TrimEnd('/'));
        _targetLanguage = MapLanguageCode(settings.TranslationTargetLanguage ?? "ar");
    }

    public async Task<string?> TranslateAsync(string word,
                    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(word)) return null;
        try
        {
            return await _client.TranslateAsync(new Translate
            {
                Source = LanguageCode.AutoDetect,
                Target = _targetLanguage,
                Text = word
            });
        }
        catch { return null; }
    }
    private static LanguageCode MapLanguageCode(string code) => code.ToLowerInvariant() switch
    {
        "ar" => LanguageCode.Arabic,
        "zh" => LanguageCode.Chinese,
        "fr" => LanguageCode.French,
        "de" => LanguageCode.German,
        "it" => LanguageCode.Italian,
        "ja" => LanguageCode.Japanese,
        "ko" => LanguageCode.Korean,
        "pt" => LanguageCode.Portuguese,
        "ru" => LanguageCode.Russian,
        "es" => LanguageCode.Spanish,
        _ => LanguageCode.English
    };
}