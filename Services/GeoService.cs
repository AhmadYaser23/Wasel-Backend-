using Microsoft.Extensions.Caching.Memory;
using ProjectWasel21.Models;
using System.Text.Json;

public class GeoService
{
    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;

    public GeoService(HttpClient http, IMemoryCache cache)
    {
        _http = http;
        _cache = cache;
    }

    public async Task<GeoResult?> GetLocationAsync(string query)
    {
        var cacheKey = $"geo_{query.ToLower()}";

        if (_cache.TryGetValue(cacheKey, out GeoResult cached))
            return cached;

        var url = $"https://nominatim.openstreetmap.org/search?q={query}&format=json&limit=1";

        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<NominatimResult[]>(json);

        if (data == null || data.Length == 0) return null;

        var result = new GeoResult
        {
            Latitude = double.Parse(data[0].lat),
            Longitude = double.Parse(data[0].lon),
            DisplayName = data[0].display_name
        };

        // ✅ Cache لمدة 30 دقيقة
        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));

        return result;
    }

    private class NominatimResult
    {
        public string lat { get; set; }
        public string lon { get; set; }
        public string display_name { get; set; }
    }
}