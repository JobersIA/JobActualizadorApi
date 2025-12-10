//╔═══════════════════════════════════════════════════════════╗
//║       ██╗ ██████╗ ██████╗ ███████╗██████╗ ███████╗        ║
//║       ██║██╔═══██╗██╔══██╗██╔════╝██╔══██╗██╔════╝        ║
//║       ██║██║   ██║██████╔╝█████╗  ██████╔╝███████╗        ║
//║  ██   ██║██║   ██║██╔══██╗██╔══╝  ██╔══██╗╚════██║        ║
//║  ╚█████╔╝╚██████╔╝██████╔╝███████╗██║  ██║███████║        ║
//║   ╚════╝  ╚═════╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝        ║
//╚═══════════════════════════════════════════════════════════╝
//10-12-2025: Program.cs
//Autor: Ramón San Félix Ramón
//Email: rsanfelix@jobers.net
//Teléfono: 626 99 09 26

using System.Reflection;
using Microsoft.OpenApi.Models;
using JobActualizadorApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ==================== CONFIGURACION ====================
builder.Services.Configure<ApiVersionSettings>(
    builder.Configuration.GetSection("ApiVersion"));

builder.Services.Configure<Dictionary<string, AppVersionInfo>>(
    builder.Configuration.GetSection("AppVersions"));

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

// Endpoint de version (lee configuracion de appsettings.json)
app.MapGet("/api/version", (IConfiguration config) =>
{
    var versionInfo = Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0";
    var cleanVersion = versionInfo.Split('+')[0];

    var apiVersionConfig = config.GetSection("ApiVersion");
    var compatibleVersions = apiVersionConfig.GetSection("CompatibleVersions").Get<string[]>() ?? ["1.0.0"];
    var minClientVersion = apiVersionConfig["MinClientVersion"] ?? "1.0.0";

    return new
    {
        version = cleanVersion,
        compatible_versions = compatibleVersions,
        min_client_version = minClientVersion,
        framework = ".NET 8.0",
        title = "JobActualizadorApi"
    };
});

app.Run();
