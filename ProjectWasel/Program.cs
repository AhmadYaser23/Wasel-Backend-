using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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
builder.Services.AddScoped<IncidentService>();
builder.Services.AddScoped<RouteService>();

// ===== JWT Authentication =====
var jwtKey = builder.Configuration["Jwt:Key"] ?? "WaselPalestineSecretKeyForJWT2026!AtLeast32Chars";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "ProjectWasel";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "ProjectWaselUsers";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
builder.Services.AddAuthorization();

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
app.UseAuthentication();
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