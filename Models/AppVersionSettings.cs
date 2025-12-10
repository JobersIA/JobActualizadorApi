//╔═══════════════════════════════════════════════════════════╗
//║       ██╗ ██████╗ ██████╗ ███████╗██████╗ ███████╗        ║
//║       ██║██╔═══██╗██╔══██╗██╔════╝██╔══██╗██╔════╝        ║
//║       ██║██║   ██║██████╔╝█████╗  ██████╔╝███████╗        ║
//║  ██   ██║██║   ██║██╔══██╗██╔══╝  ██╔══██╗╚════██║        ║
//║  ╚█████╔╝╚██████╔╝██████╔╝███████╗██║  ██║███████║        ║
//║   ╚════╝  ╚═════╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝        ║
//╚═══════════════════════════════════════════════════════════╝
//10-12-2025: AppVersionSettings.cs
//Autor: Ramón San Félix Ramón
//Email: rsanfelix@jobers.net
//Teléfono: 626 99 09 26

namespace JobActualizadorApi.Models
{
    /// <summary>
    /// Configuracion de version de la API (para endpoint /api/version)
    /// </summary>
    public class ApiVersionSettings
    {
        public string[] CompatibleVersions { get; set; } = ["1.0.0"];
        public string MinClientVersion { get; set; } = "1.0.0";
    }

    /// <summary>
    /// Configuracion de versiones de la app por plataforma
    /// </summary>
    public class AppVersionSettings
    {
        public Dictionary<string, AppVersionInfo> Versions { get; set; } = new();
    }
}
