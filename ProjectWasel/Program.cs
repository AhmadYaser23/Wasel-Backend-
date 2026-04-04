using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using project.Helper;
using ProjectWasel.Data;
using ProjectWasel.Helper;
using ProjectWasel.Repositories;
using ProjectWasel.Repositres;
using ProjectWasel.Services;
using System.Net.Http.Headers;
using ProjectWasel.Profiles;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);
var apiKey = builder.Configuration["OpenWeatherApiKey"];
Console.WriteLine($"OpenWeather API Key: {apiKey}");

// ===== Add Services =====
builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== DbContext (CHANGED TO POSTGRESQL) =====
builder.Services.AddDbContext<WaselContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== Repositories =====
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICheckpointRepository, CheckpointRepository>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();

// ===== Services =====
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<RouteService>();

// ===== Helpers =====
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddSingleton<PasswordHasher>();

// ===== HTTP Clients with Polly =====
builder.Services.AddHttpClient<GeoService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "ProjectWaselApp");
});

builder.Services.AddHttpClient<WeatherService>(client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
});
builder.Services.AddScoped<ExternalApiService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
// ===== Profiles =====
var app = builder.Build();




// ===== Middleware =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/test-weather", async (IConfiguration config) =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/")
    };
    var weatherService = new WeatherService(httpClient, config);
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