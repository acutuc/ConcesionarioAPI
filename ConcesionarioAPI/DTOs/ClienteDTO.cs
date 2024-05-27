using System.ComponentModel.DataAnnotations;

namespace ConcesionarioAPI.DTOs
{
    public class ClienteDTO
    {
        public int ClienteID { get; set; }

        [Required]
        public string NombreCliente { get; set; }

        [Required]
        public string ApellidosCliente { get; set; }

        [Required]
        public string TelefonoCliente { get; set; }

        [Required]
        public string DNI { get; set; }
    }
}
