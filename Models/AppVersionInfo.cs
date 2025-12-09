namespace JobActualizadorApi.Models
{
    public class AppVersionInfo
    {
        public required string VersionActual { get; set; }
        public required string VersionMinima { get; set; }
        public required string UrlDescarga { get; set; }
        public required string NotasVersion { get; set; }
        public bool ActualizacionForzada { get; set; }
    }
}
