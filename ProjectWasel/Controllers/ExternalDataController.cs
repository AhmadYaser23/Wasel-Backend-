using Microsoft.AspNetCore.Mvc;
using ProjectWasel.Models;
using ProjectWasel.Services;
using ProjectWasel.Data;
using System.Text.Json;

namespace ProjectWasel.Controllers
{
    [ApiController]
    [Route("api/external")]
    public class ExternalDataController : ControllerBase
    {
        private readonly ExternalApiService _externalApi;
        private readonly WaselContext _context;

        public ExternalDataController(ExternalApiService externalApi, WaselContext context)
        {
            _externalApi = externalApi;
            _context = context;
        }

        [HttpGet("location-weather")]
        public async Task<ActionResult> GetLocationWeather([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query is required.");

            var data = await _externalApi.GetLocationWeatherDataAsync(query);
            if (data == null)
                return NotFound("Data not available.");

            _context.ExternalData.Add(data);
            await _context.SaveChangesAsync();

            return Ok(JsonSerializer.Deserialize<object>(data.JsonData));
        }
    }
}