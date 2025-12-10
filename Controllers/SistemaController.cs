//╔═══════════════════════════════════════════════════════════╗
//║       ██╗ ██████╗ ██████╗ ███████╗██████╗ ███████╗        ║
//║       ██║██╔═══██╗██╔══██╗██╔════╝██╔══██╗██╔════╝        ║
//║       ██║██║   ██║██████╔╝█████╗  ██████╔╝███████╗        ║
//║  ██   ██║██║   ██║██╔══██╗██╔══╝  ██╔══██╗╚════██║        ║
//║  ╚█████╔╝╚██████╔╝██████╔╝███████╗██║  ██║███████║        ║
//║   ╚════╝  ╚═════╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝        ║
//╚═══════════════════════════════════════════════════════════╝
//10-12-2025: SistemaController.cs
//Autor: Ramón San Félix Ramón
//Email: rsanfelix@jobers.net
//Teléfono: 626 99 09 26

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using JobActualizadorApi.Models;

namespace JobActualizadorApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SistemaController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        // Almacenamiento en memoria para las versiones (inicializado desde appsettings.json)
        private static Dictionary<string, AppVersionInfo>? _versionStore;
        private static readonly object _lock = new();

        public SistemaController(
            IWebHostEnvironment env,
            IConfiguration configuration,
            IOptions<Dictionary<string, AppVersionInfo>> appVersionsOptions)
        {
            _env = env;
            _configuration = configuration;

            // Inicializar el store solo una vez desde la configuracion
            lock (_lock)
            {
                if (_versionStore == null)
                {
                    _versionStore = appVersionsOptions.Value ?? GetDefaultVersions();
                }
            }
        }

        private static Dictionary<string, AppVersionInfo> GetDefaultVersions()
        {
            return new Dictionary<string, AppVersionInfo>
            {
                ["Android"] = new AppVersionInfo
                {
                    VersionActual = "1.0.0",
                    VersionMinima = "1.0.0",
                    UrlDescarga = "",
                    NotasVersion = "Version por defecto",
                    ActualizacionForzada = false
                },
                ["iOS"] = new AppVersionInfo
                {
                    VersionActual = "1.0.0",
                    VersionMinima = "1.0.0",
                    UrlDescarga = "",
                    NotasVersion = "Version por defecto",
                    ActualizacionForzada = false
                }
            };
        }

        /// <summary>
        /// Obtiene la informacion de version para una plataforma
        /// </summary>
        [HttpGet("{plataforma}")]
        [ProducesResponseType(typeof(AppVersionInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAppVersion(string plataforma)
        {
            if (_versionStore!.TryGetValue(plataforma, out var versionInfo))
            {
                return Ok(versionInfo);
            }

            return NotFound(new { error = $"Plataforma '{plataforma}' no encontrada" });
        }

        /// <summary>
        /// Actualiza la informacion de version para una plataforma (en memoria)
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateAppVersion([FromBody] UpdateAppVersionRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { error = "El request no puede ser nulo" });
            }

            _versionStore![request.Plataforma] = new AppVersionInfo
            {
                VersionActual = request.VersionActual,
                VersionMinima = request.VersionMinima,
                UrlDescarga = request.UrlDescarga,
                NotasVersion = request.NotasVersion ?? "",
                ActualizacionForzada = request.ActualizacionForzada
            };

            return Ok(new { success = true, message = "Version actualizada correctamente (en memoria)" });
        }

        /// <summary>
        /// Recarga la configuracion desde appsettings.json
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult ReloadConfig()
        {
            lock (_lock)
            {
                var section = _configuration.GetSection("AppVersions");
                var newConfig = section.Get<Dictionary<string, AppVersionInfo>>();

                if (newConfig != null && newConfig.Count > 0)
                {
                    _versionStore = newConfig;
                    return Ok(new { success = true, message = "Configuracion recargada desde appsettings.json", versions = _versionStore });
                }

                return Ok(new { success = false, message = "No se encontro configuracion, manteniendo valores actuales" });
            }
        }

        /// <summary>
        /// Obtiene todas las versiones configuradas
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Dictionary<string, AppVersionInfo>), StatusCodes.Status200OK)]
        public IActionResult GetAllVersions()
        {
            return Ok(_versionStore);
        }

        /// <summary>
        /// Descarga un archivo APK
        /// </summary>
        [HttpGet("{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Download(string fileName)
        {
            // Solo permitir archivos .apk
            if (!fileName.EndsWith(".apk", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { error = "Solo se permiten archivos .apk" });
            }

            var downloadsPath = Path.Combine(_env.ContentRootPath, "Downloads");
            var filePath = Path.GetFullPath(Path.Combine(downloadsPath, fileName));

            // Validar path traversal
            if (!filePath.StartsWith(downloadsPath, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { error = "Ruta no permitida" });
            }

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { error = $"Archivo no encontrado: {fileName}" });
            }

            const string contentType = "application/vnd.android.package-archive";
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, contentType, fileName);
        }
    }
}
