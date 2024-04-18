using System.ComponentModel.DataAnnotations;

namespace ConcesionarioAPI.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        [Required]
        public required string NombreUsuario { get; set; }
        [Required]
        public required string ClaveUsuario { get; set; }
        public string? TipoUsuario { get; set; }
        public List<Sucursal>? Sucursales { get; set; }
    }
}
