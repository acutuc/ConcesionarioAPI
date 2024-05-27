using System.ComponentModel.DataAnnotations;

namespace ConcesionarioAPI.DTOs
{
    public class VehiculoDTO
    {
        public int VehiculoID { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        public string Modelo { get; set; }

        public int Anio { get; set; }

        public int Precio { get; set; }
    }
}
