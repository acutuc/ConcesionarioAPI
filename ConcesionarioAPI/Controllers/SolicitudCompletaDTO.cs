using ConcesionarioAPI.DTOs;

namespace ConcesionarioAPI.Controllers
{
    public class SolicitudCompletaDTO
    {
        public int SolicitudID { get; set; }
        public string Estado { get; set; }
        public string TipoSolicitud { get; set; }
        public int SucursalID { get; set; }
        public int VehiculoID { get; set; }
        public int ClienteID { get; set; }
        public SucursalDTO Sucursal { get; set; }
        public VehiculoDTO Vehiculo { get; set; }
    }
}
