using Swashbuckle.AspNetCore.Annotations;

namespace ConcesionarioAPI.DTOs
{
    public class SucursalDTO
    {
        //Esta anotación indica a Swagger que el campo no es editable
        [SwaggerSchema(ReadOnly = true)] 
        public int SucursalID { get; set; }
        public int UsuarioID { get; set; }
        public string NombreSucursal { get; set; }
        public string Ubicacion { get; set; }
    }
}
