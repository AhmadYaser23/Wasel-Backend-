using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using project.Helper;
using ProjectWasel.Data;
using ProjectWasel.Helper;
using ProjectWasel.Models.Repositres;
using ProjectWasel.Repositories;
using ProjectWasel.Repositres;
using ProjectWasel.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Config
// =========================
var apiKey = builder.Configuration["OpenWeatherApiKey"];
Console.WriteLine($"OpenWeather API Key: {apiKey}");

// =========================
// Controllers + Swagger
// =========================
builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========================
// DbContext
// =========================
builder.Services.AddDbContext<WaselContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================
// Repositories
// =========================
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICheckpointRepository, CheckpointRepository>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();

// ===== Reports (IMPORTANT) =====
builder.Services.AddScoped<IReportRepository, ReportRepository>();

builder.Services.AddScoped<AuthRepository>();

// =========================
// Services
// =========================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<RouteService>();
builder.Services.AddScoped<ReportService>(); // ✔ مهم جداً

builder.Services.AddScoped<ExternalApiService>();

// =========================
// Helpers
// =========================
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddSingleton<PasswordHasher>();

// =========================
// Memory Cache
// =========================
builder.Services.AddMemoryCache();

// =========================
// HTTP Clients
// =========================
builder.Services.AddHttpClient<GeoService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "ProjectWaselApp");
});

builder.Services.AddHttpClient<WeatherService>(client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
});




builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// =========================
// Build App
// =========================
var app = builder.Build();

// =========================
// Middleware
// =========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// =========================
// Test Endpoints
// =========================
app.MapGet("/test-weather", async (WeatherService weatherService) =>
{
    var weather = await weatherService.GetWeatherAsync(31.5, 34.47);
    return Results.Ok(weather);
});

app.MapGet("/test-weather1", async (ExternalApiService externalApi) =>
{
    var data = await externalApi.GetLocationWeatherDataAsync("London");
    if (data == null) return Results.NotFound("Data not available.");
    return Results.Ok(data);
});

app.Run();