namespace ConcesionarioAPI.DTOs
{
    public class ActualizarEstadoSolicitudDTO
    {
        public string Estado { get; set; }
        public int ClienteID { get; set; }
        public bool PrecioActualizado { get; set; } = false;
    }
}
