namespace ConcesionarioAPI.DTOs
{
    public class SolicitudVendidaDTO
    {
        public int SolicitudID { get; set; }
        public string NombreSucursal { get; set; }
        public string UbicacionSucursal { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidosCliente { get; set; }
        public string TelefonoCliente { get; set; }
    }
}