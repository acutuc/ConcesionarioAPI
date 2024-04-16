﻿using System.ComponentModel.DataAnnotations;

namespace ConcesionarioAPI.Models
{
    public class Sucursal
    {
        [Key]
        public int SucursalID { get; set; }
        public int UsuarioID { get; set; }
        public string NombreSucursal { get; set; }
        public string Ubicacion { get; set; }
        public Usuario Usuario { get; set; }
        public List<Vehiculo>? Vehiculos { get; set; }

    }
}