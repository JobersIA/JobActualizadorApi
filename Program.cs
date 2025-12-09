using System.Reflection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ==================== CORS ====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ==================== CONTROLADORES ====================
builder.Services.AddControllers();

// ==================== SWAGGER ====================
builder.Services.AddEndpointsApiExplorer();

var versionInfo = Assembly.GetExecutingAssembly()
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0";
var version = versionInfo.Split('+')[0];

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = version,
        Title = "JobActualizadorApi",
        Description = "API de prueba para sistema de autoactualizacion"
    });
});

// ==================== CONSTRUCCION DEL APP ====================
var app = builder.Build();

// ==================== PIPELINE ====================
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.MapControllers();

// Endpoint raiz
app.MapGet("/api", () => "JobActualizadorApi funcionando y lista para recibir solicitudes...");

// Endpoint de version
app.MapGet("/api/version", () =>
{
    var versionInfo = Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0";
    var cleanVersion = versionInfo.Split('+')[0];

    return new
    {
        version = cleanVersion,
        compatible_versions = new[] { "1.0.0" },
        min_client_version = "1.0.0",
        framework = ".NET 8.0",
        title = "JobActualizadorApi"
    };
});

app.Run();
