using Microsoft.Extensions.Caching.Memory;
using ProjectWasel21.Models;
using System.Text.Json;

public class WeatherService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly IMemoryCache _cache;

    public WeatherService(HttpClient http, IConfiguration config, IMemoryCache cache)
    {
        _http = http;
        _cache = cache;
        _apiKey = config["OpenWeatherApiKey"] ?? throw new Exception("API key missing");
        _http.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
    }

    public async Task<WeatherResult?> GetWeatherAsync(double lat, double lon)
    {
        var cacheKey = $"weather_{lat}_{lon}";

        if (_cache.TryGetValue(cacheKey, out WeatherResult cached))
            return cached;

        var url = $"weather?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";

        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = doc.RootElement;

        var result = new WeatherResult
        {
            Status = root.GetProperty("weather")[0].GetProperty("main").GetString() ?? "Unknown",
            Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
            Humidity = root.GetProperty("main").GetProperty("humidity").GetDouble()
        };

        // ✅ Cache لمدة 10 دقائق (weather بتتغير بسرعة)
        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

        return result;
    }
}