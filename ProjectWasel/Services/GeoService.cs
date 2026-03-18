using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ProjectWasel.Models;

namespace ProjectWasel.Services
{
    public class GeoService
    {
        private readonly HttpClient _http;

        public GeoService(HttpClient http) => _http = http;

        public async Task<GeoResult?> GetLocationAsync(string query)
        {
            var url = $"https://nominatim.openstreetmap.org/search?q={query}&format=json&limit=1";
            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<NominatimResult[]>(json);
            if (data == null || data.Length == 0) return null;

            return new GeoResult
            {
                Latitude = double.Parse(data[0].lat),
                Longitude = double.Parse(data[0].lon),
                DisplayName = data[0].display_name
            };
        }

        private class NominatimResult
        {
            public string lat { get; set; }
            public string lon { get; set; }
            public string display_name { get; set; }
        }
    }
}