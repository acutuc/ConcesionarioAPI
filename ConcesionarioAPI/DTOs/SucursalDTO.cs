using Swashbuckle.AspNetCore.Annotations;

namespace ConcesionarioAPI.DTOs
{
    public class SucursalDTO
    {
        [SwaggerSchema(ReadOnly = true)] // Esta anotación indica a Swagger que el campo no es editable
        public int SucursalID { get; set; }

        public int UsuarioID { get; set; }
        public string NombreSucursal { get; set; }
        public string Ubicacion { get; set; }
    }
}
