using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
// API VERSIONING
// =========================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// =========================
// SWAGGER
// =========================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========================
// DB CONTEXT
// =========================
builder.Services.AddDbContext<WaselContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================
// AUTO MAPPER
// =========================
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<CheckpointProfile>();
    cfg.AddProfile<IncidentProfile>();
});

// =========================
// REPOSITORIES
// =========================
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICheckpointRepository, CheckpointRepository>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<AuthRepository>();

// =========================
// SERVICES
// =========================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<RouteService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<ExternalApiService>();
builder.Services.AddScoped<IncidentService>(); // ✅ حل مشكلة DI
builder.Services.AddScoped<UserService>();

// =========================
// HELPERS
// =========================
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddSingleton<PasswordHasher>();

// =========================
// MEMORY CACHE
// =========================
builder.Services.AddMemoryCache();

// =========================
// HTTP CLIENTS
// =========================
builder.Services.AddHttpClient<GeoService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "ProjectWasel21App");
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
// JWT AUTH
// =========================
builder.Services.AddAuthentication("Bearer")
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
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
// =========================
// BUILD APP
// =========================
var app = builder.Build();


// =========================
// MIGRATION + SEED
// =========================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<WaselContext>();

        context.Database.Migrate();
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