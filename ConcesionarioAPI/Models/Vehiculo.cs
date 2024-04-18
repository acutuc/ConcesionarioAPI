namespace ConcesionarioAPI.Models
{
    public class Vehiculo
    {
        public int VehiculoID { get; set; }
        public int SucursalID { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public int Anio { get; set; }
        public double Precio { get; set; }
        public bool Vendido { get; set; }
        public Sucursal? Sucursal { get; set; }
    }
}
