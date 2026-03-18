using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ProjectWasel.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace ProjectWasel.Services
{
    public class WeatherService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public WeatherService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["OpenWeatherApiKey"] ?? throw new Exception("API key missing");
            _http.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
        }

        public async Task<WeatherResult?> GetWeatherAsync(double lat, double lon)
        {
            var url = $"weather?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";
            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var root = doc.RootElement;

            return new WeatherResult
            {
                Status = root.GetProperty("weather")[0].GetProperty("main").GetString() ?? "Unknown",
                Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
                Humidity = root.GetProperty("main").GetProperty("humidity").GetDouble()
            };
        }
    }
}