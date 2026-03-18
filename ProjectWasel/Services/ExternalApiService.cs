using ProjectWasel.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectWasel.Services
{
    public class ExternalApiService
    {
        private readonly GeoService _geoService;
        private readonly WeatherService _weatherService;

        public ExternalApiService(GeoService geoService, WeatherService weatherService)
        {
            _geoService = geoService;
            _weatherService = weatherService;
        }

        public async Task<ExternalData?> GetLocationWeatherDataAsync(string query)
        {
            var geo = await _geoService.GetLocationAsync(query);
            if (geo == null) return null;

            var weather = await _weatherService.GetWeatherAsync(geo.Latitude, geo.Longitude);
            if (weather == null) return null;

            var combined = new
            {
                Location = geo,
                Weather = weather
            };

            return new ExternalData
            {
                Source = "Geo + Weather API",
                ExternalKey = query,
                JsonData = JsonSerializer.Serialize(combined),
                FetchedAt = DateTime.UtcNow
            };
        }
    }
}