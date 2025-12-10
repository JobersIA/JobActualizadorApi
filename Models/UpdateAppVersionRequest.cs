//╔═══════════════════════════════════════════════════════════╗
//║       ██╗ ██████╗ ██████╗ ███████╗██████╗ ███████╗        ║
//║       ██║██╔═══██╗██╔══██╗██╔════╝██╔══██╗██╔════╝        ║
//║       ██║██║   ██║██████╔╝█████╗  ██████╔╝███████╗        ║
//║  ██   ██║██║   ██║██╔══██╗██╔══╝  ██╔══██╗╚════██║        ║
//║  ╚█████╔╝╚██████╔╝██████╔╝███████╗██║  ██║███████║        ║
//║   ╚════╝  ╚═════╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝        ║
//╚═══════════════════════════════════════════════════════════╝
//10-12-2025: UpdateAppVersionRequest.cs
//Autor: Ramón San Félix Ramón
//Email: rsanfelix@jobers.net
//Teléfono: 626 99 09 26

namespace JobActualizadorApi.Models
{
    public class UpdateAppVersionRequest
    {
        public required string Plataforma { get; set; }
        public required string VersionActual { get; set; }
        public required string VersionMinima { get; set; }
        public required string UrlDescarga { get; set; }
        public string? NotasVersion { get; set; }
        public bool ActualizacionForzada { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
