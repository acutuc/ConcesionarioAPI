using System.ComponentModel.DataAnnotations;

namespace ConcesionarioAPI.DTOs
{
    public class UsuarioDTO
    {
        public int UsuarioID { get; set; }
        [Required]
        public string NombreUsuario { get; set; }
        [Required]
        public string ClaveUsuario { get; set; }
        public string? TipoUsuario { get; set; }
    }
}