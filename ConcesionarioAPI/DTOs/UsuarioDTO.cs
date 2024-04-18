namespace ConcesionarioAPI.DTOs
{
    public class UsuarioDTO
    {
        public int UsuarioID { get; set; }
        public required string NombreUsuario { get; set; }
        public required string ClaveUsuario { get; set; }
        public string? TipoUsuario { get; set; }
    }
}