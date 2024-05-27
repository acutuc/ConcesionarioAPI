using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcesionarioAPI.Context;
using ConcesionarioAPI.Models;
using ConcesionarioAPI.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcesionarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehiculoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Vehiculo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehiculoDTO>>> GetVehiculos()
        {
            var vehiculos = await _context.Vehiculos.Select(v => new VehiculoDTO
            {
                VehiculoID = v.VehiculoID,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Anio = v.Anio,
                Precio = v.Precio
            }).ToListAsync();

            return vehiculos;
        }

        // GET: api/Vehiculo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VehiculoDTO>> GetVehiculo(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);

            if (vehiculo == null)
            {
                return NotFound();
            }

            var vehiculoDTO = new VehiculoDTO
            {
                VehiculoID = vehiculo.VehiculoID,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Anio = vehiculo.Anio,
                Precio = vehiculo.Precio
            };

            return vehiculoDTO;
        }

        // POST: api/Vehiculo
        [HttpPost]
        public async Task<ActionResult<VehiculoDTO>> PostVehiculo(VehiculoDTO vehiculoDTO)
        {
            var vehiculo = new Vehiculo
            {
                Marca = vehiculoDTO.Marca,
                Modelo = vehiculoDTO.Modelo,
                Anio = vehiculoDTO.Anio,
                Precio = vehiculoDTO.Precio
            };

            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();

            vehiculoDTO.VehiculoID = vehiculo.VehiculoID;

            return CreatedAtAction("GetVehiculo", new { id = vehiculo.VehiculoID }, vehiculoDTO);
        }

        // PUT: api/Vehiculo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehiculo(int id, VehiculoDTO vehiculoDTO)
        {
            if (id != vehiculoDTO.VehiculoID)
            {
                return BadRequest();
            }

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            vehiculo.Marca = vehiculoDTO.Marca;
            vehiculo.Modelo = vehiculoDTO.Modelo;
            vehiculo.Anio = vehiculoDTO.Anio;
            vehiculo.Precio = vehiculoDTO.Precio;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Vehiculo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehiculo(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Obtenemos todos los vehículos de una sucursal en concreto:
        [HttpGet("VehiculosEnSucursal/{sucursalId}")]
        public async Task<ActionResult<IEnumerable<VehiculoDTO>>> GetVehiculosEnSucursal(int sucursalId)
        {
            var solicitudesEnSucursal = await _context.Solicitudes
                .Where(s => s.SucursalID == sucursalId && s.Estado == "aprobada")
                .ToListAsync();

            var vehiculosIdsEnSucursal = solicitudesEnSucursal.Select(s => s.VehiculoID).Distinct();

            var vehiculosEnSucursal = await _context.Vehiculos
                .Where(v => vehiculosIdsEnSucursal.Contains(v.VehiculoID))
                .Select(v => new VehiculoDTO
                {
                    VehiculoID = v.VehiculoID,
                    Marca = v.Marca,
                    Modelo = v.Modelo,
                    Anio = v.Anio,
                    Precio = v.Precio
                })
                .ToListAsync();

            return vehiculosEnSucursal;
        }
    }
}
