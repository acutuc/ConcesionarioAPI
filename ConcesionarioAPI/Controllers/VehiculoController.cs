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
                SucursalID = v.SucursalID,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Anio = v.Anio,
                Precio = v.Precio,
                Vendido = v.Vendido
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
                SucursalID = vehiculo.SucursalID,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Anio = vehiculo.Anio,
                Precio = vehiculo.Precio,
                Vendido = vehiculo.Vendido
            };

            return vehiculoDTO;
        }

        // POST: api/Vehiculo
        [HttpPost]
        public async Task<ActionResult<VehiculoDTO>> PostVehiculo(VehiculoDTO vehiculoDTO)
        {
            var vehiculo = new Vehiculo
            {
                SucursalID = vehiculoDTO.SucursalID,
                Marca = vehiculoDTO.Marca,
                Modelo = vehiculoDTO.Modelo,
                Anio = vehiculoDTO.Anio,
                Precio = vehiculoDTO.Precio,
                Vendido = vehiculoDTO.Vendido
            };

            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();

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

            vehiculo.SucursalID = vehiculoDTO.SucursalID;
            vehiculo.Marca = vehiculoDTO.Marca;
            vehiculo.Modelo = vehiculoDTO.Modelo;
            vehiculo.Anio = vehiculoDTO.Anio;
            vehiculo.Precio = vehiculoDTO.Precio;
            vehiculo.Vendido = vehiculoDTO.Vendido;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiculoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

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

        private bool VehiculoExists(int id)
        {
            return _context.Vehiculos.Any(e => e.VehiculoID == id);
        }
    }
}
