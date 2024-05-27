namespace ConcesionarioAPI.DTOs
{
    public class SolicitudDTO
    {
        public int SolicitudID { get; set; }
        public string? Estado { get; set; }
        public string? TipoSolicitud { get; set; }
        public int SucursalID { get; set; }
        public int VehiculoID { get; set; }
        public int ClienteID{ get; set; }
    }
}
