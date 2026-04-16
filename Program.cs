using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProjectWasel21.Helper;
using ProjectWasel21.Data;
using ProjectWasel21.Helper;
using ProjectWasel21.Models.Repositres;
using ProjectWasel21.Profiles;
using ProjectWasel21.Repositories;
using ProjectWasel21.Repositres;
using ProjectWasel21.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Controllers + JSON FIX
// =========================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// =========================
// API Versioning
// =========================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// =========================
// Swagger
// =========================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========================
// DbContext
// =========================
builder.Services.AddDbContext<WaselContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================
// AUTO MAPPER (FIX)
// =========================
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<CheckpointProfile>();
    cfg.AddProfile<IncidentProfile>();
});// =========================
// Repositories
// =========================
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICheckpointRepository, CheckpointRepository>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<AuthRepository>();

// =========================
// Services
// =========================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<RouteService>();
builder.Services.AddScoped<ReportService>();
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
    client.DefaultRequestHeaders.Add("User-Agent", "ProjectWasel21App");
});

builder.Services.AddHttpClient<WeatherService>(client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
});

// =========================
// JWT AUTH
// =========================
builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"] ?? "default_secret_key"
                ))
        };
    });

// =========================
// BUILD APP
// =========================
var app = builder.Build();

// =========================
// APPLY MIGRATIONS + SEED 🔥
// =========================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<WaselContext>();

        // يعمل إنشاء DB + Migration
        context.Database.Migrate();

        // يشغل Seed
        context.Seed();
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Error during migration/seed:");
        Console.WriteLine(ex.Message);
    }
}

// =========================
// MIDDLEWARE
// =========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();