using System.ComponentModel.DataAnnotations;

namespace ConcesionarioAPI.Models
{
    public class Cliente
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
        public List<Solicitud>? Solicitudes { get; set; }

    }
}
