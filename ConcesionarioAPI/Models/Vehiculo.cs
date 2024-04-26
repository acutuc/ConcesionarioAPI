using System.ComponentModel.DataAnnotations;

namespace ConcesionarioAPI.Models
{
    public class Vehiculo
    {
        public int VehiculoID { get; set; }
        public int SucursalID { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        public string Modelo { get; set; }

        public int Anio { get; set; }
        public double Precio { get; set; }
        public bool Vendido { get; set; }
        public Sucursal Sucursal { get; set; }
    }
}
