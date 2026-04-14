using ProjectWasel.Models;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace ProjectWasel.Services
{
    public class ExternalApiService
    {
        private readonly GeoService _geoService;
        private readonly WeatherService _weatherService;
        private readonly IMemoryCache _cache;

        public ExternalApiService(
            GeoService geoService,
            WeatherService weatherService,
            IMemoryCache cache)
        {
            _geoService = geoService;
            _weatherService = weatherService;
            _cache = cache;
        }

        public async Task<ExternalData?> GetLocationWeatherDataAsync(string query)
        {
            var cacheKey = $"full_{query.ToLower()}";

            // ✅ Check cache
            if (_cache.TryGetValue(cacheKey, out string cachedJson))
            {
                return new ExternalData
                {
                    Source = "Geo + Weather API",
                    ExternalKey = query,
                    JsonData = cachedJson,
                    FetchedAt = DateTime.UtcNow
                };
            }

            // ❗ Fetch from APIs
            var geo = await _geoService.GetLocationAsync(query);
            if (geo == null) return null;

            var weather = await _weatherService.GetWeatherAsync(geo.Latitude, geo.Longitude);
            if (weather == null) return null;

            var combined = new
            {
                Location = geo,
                Weather = weather
            };

            var json = JsonSerializer.Serialize(combined);

            // ✅ Store in cache
            _cache.Set(cacheKey, json, TimeSpan.FromMinutes(15));

            return new ExternalData
            {
                Source = "Geo + Weather API",
                ExternalKey = query,
                JsonData = json,
                FetchedAt = DateTime.UtcNow
            };
        }
    }
}