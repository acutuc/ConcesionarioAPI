namespace ConcesionarioAPI.Models
{
    public class Solicitud
    {
        public int SolicitudID { get; set; }
        public string? Estado { get; set; }
        public string? TipoSolicitud { get; set; }
        public int SucursalID { get; set; }
        public Sucursal Sucursal { get; set; }
        public int VehiculoID { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public int ClienteID { get; set; }
        public Cliente Cliente{ get; set; }
    }
}
