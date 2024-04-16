namespace ConcesionarioAPI.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string ClaveUsuario { get; set; }
        public string TipoUsuario { get; set; }
        public List<Sucursal>? Sucursales { get; set; }
    }
}
