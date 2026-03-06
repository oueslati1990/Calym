using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Caly.Core.Services.Interfaces;

namespace Caly.Core.Services;

[JsonSerializable(typeof(MyMemoryResponse))]
[JsonSerializable(typeof(MyMemoryData))]
internal partial class MyMemoryJsonContext : JsonSerializerContext;

internal sealed class LibreTranslateService : ITranslationService
{
    private static readonly HttpClient _http = new();
    private readonly string _targetLanguage;

    public LibreTranslateService(ISettingsService settingsService)
    {
        var settings = settingsService.GetSettings();
        _targetLanguage = settings.TranslationTargetLanguage ?? "ar";
    }

    public async Task<string?> TranslateAsync(string word, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(word)) return null;
        try
        {
            var url = $"https://api.mymemory.translated.net/get?q={Uri.EscapeDataString(word)}&langpair=en|{_targetLanguage}";
            var response = await _http.GetFromJsonAsync(url, MyMemoryJsonContext.Default.MyMemoryResponse, cancellationToken);
            return response?.ResponseData?.TranslatedText;
        }
        catch (Exception ex) { System.Console.WriteLine($"[Translation] ERROR: {ex.Message}"); return null; }
    }
}

internal sealed class MyMemoryResponse
{
    [JsonPropertyName("responseData")]
    public MyMemoryData? ResponseData { get; set; }
}

internal sealed class MyMemoryData
{
    [JsonPropertyName("translatedText")]
    public string? TranslatedText { get; set; }
}
