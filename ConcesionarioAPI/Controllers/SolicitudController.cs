using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcesionarioAPI.Context;
using ConcesionarioAPI.Models;
using ConcesionarioAPI.DTOs;

namespace ConcesionarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SolicitudController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Solicitud
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudDTO>>> GetSolicitudes()
        {
            var solicitudes = await _context.Solicitudes
                .Include(s => s.Sucursal)
                .Include(s => s.Vehiculo)
                .Include(s => s.Cliente)
                .Select(s => new SolicitudDTO
                {
                    SolicitudID = s.SolicitudID,
                    Estado = s.Estado,
                    TipoSolicitud = s.TipoSolicitud,
                    SucursalID = s.SucursalID,
                    VehiculoID = s.VehiculoID,
                    ClienteID = s.ClienteID
                })
                .ToListAsync();

            return solicitudes;
        }

        // GET: api/Solicitud/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudDTO>> GetSolicitud(int id)
        {
            var solicitud = await _context.Solicitudes
                .Include(s => s.Sucursal)
                .Include(s => s.Vehiculo)
                .Include(s => s.Cliente)
                .FirstOrDefaultAsync(s => s.SolicitudID == id);

            if (solicitud == null)
            {
                return NotFound();
            }

            var solicitudDTO = new SolicitudDTO
            {
                SolicitudID = solicitud.SolicitudID,
                Estado = solicitud.Estado,
                TipoSolicitud = solicitud.TipoSolicitud,
                SucursalID = solicitud.SucursalID,
                VehiculoID = solicitud.VehiculoID,
                ClienteID = solicitud.ClienteID
            };

            return solicitudDTO;
        }

        // POST: api/Solicitud
        [HttpPost]
        public async Task<ActionResult<SolicitudDTO>> PostSolicitud(SolicitudDTO solicitudDTO)
        {
            //Verificamos si la sucursal existe
            var sucursalExistente = await _context.Sucursales.FindAsync(solicitudDTO.SucursalID);
            if (sucursalExistente == null)
            {
                return BadRequest($"No se encontró ninguna sucursal con el ID {solicitudDTO.SucursalID}.");
            }

            //Verificamos si el vehículo existe
            var vehiculoExistente = await _context.Vehiculos.FindAsync(solicitudDTO.VehiculoID);
            if (vehiculoExistente == null)
            {
                return BadRequest($"No se encontró ningún vehículo con el ID {solicitudDTO.VehiculoID}.");
            }

            //Verificamos si el cliente existe
            var clienteExistente = await _context.Clientes.FindAsync(solicitudDTO.ClienteID);
            if (clienteExistente == null)
            {
                return BadRequest($"No se encontró ningún cliente con el ID {solicitudDTO.ClienteID}.");
            }

            //Creamos una nueva solicitud con los datos proporcionados
            var solicitud = new Solicitud
            {
                Estado = solicitudDTO.Estado,
                TipoSolicitud = solicitudDTO.TipoSolicitud,
                SucursalID = solicitudDTO.SucursalID,
                VehiculoID = solicitudDTO.VehiculoID,
                ClienteID = solicitudDTO.ClienteID
            };

            //Agregamos la nueva solicitud al contexto y guardamos los cambios en la base de datos
            _context.Solicitudes.Add(solicitud);
            await _context.SaveChangesAsync();

            //Devolvemos la solicitud creada como resultado
            return CreatedAtAction(nameof(GetSolicitud), new { id = solicitud.SolicitudID }, solicitudDTO);
        }

        //EN EL MÉTODO PUT, SOLO ACTUALIZAREMOS EL ESTADO DE LA SOLICITUD:
        // PUT: api/Solicitud/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitud(int id, ActualizarEstadoSolicitudDTO actualizarEstadoSolicitudDTO)
        {
            var solicitud = await _context.Solicitudes.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound($"No se encontró ninguna solicitud con el ID {id}.");
            }

            //ACTUALIZAMOS SÓLO EL ESTADO:
            solicitud.Estado = actualizarEstadoSolicitudDTO.Estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudExists(id))
                {
                    return NotFound($"La solicitud con el ID {id} ya no existe en la base de datos.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Solicitud/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitud(int id)
        {
            var solicitud = await _context.Solicitudes.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound($"No se encontró ninguna solicitud con el ID {id}.");
            }

            //Eliminamos la solicitud del contexto y guardamos los cambios en la base de datos
            _context.Solicitudes.Remove(solicitud);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Devolvemos todas aquellas Solicitudes que su Estado sea "pendiente":
        // GET: api/Solicitud/NoAceptadas
        [HttpGet("NoAceptadasCompletas")]
        public async Task<ActionResult<IEnumerable<SolicitudCompletaDTO>>> GetSolicitudesNoAceptadasCompletas()
        {
            var solicitudesNoAceptadas = await _context.Solicitudes
                .Include(s => s.Sucursal)
                .Include(s => s.Vehiculo)
                .Where(s => s.Estado == "pendiente")
                .Select(s => new SolicitudCompletaDTO
                {
                    SolicitudID = s.SolicitudID,
                    Estado = s.Estado,
                    TipoSolicitud = s.TipoSolicitud,
                    SucursalID = s.SucursalID,
                    VehiculoID = s.VehiculoID,
                    ClienteID = s.ClienteID,
                    Sucursal = new SucursalDTO
                    {
                        SucursalID = s.Sucursal.SucursalID,
                        UsuarioID = s.Sucursal.UsuarioID,
                        NombreSucursal = s.Sucursal.NombreSucursal,
                        Ubicacion = s.Sucursal.Ubicacion
                    },
                    Vehiculo = new VehiculoDTO
                    {
                        VehiculoID = s.Vehiculo.VehiculoID,
                        Marca = s.Vehiculo.Marca,
                        Modelo = s.Vehiculo.Modelo,
                        Anio = s.Vehiculo.Anio,
                        Precio = s.Vehiculo.Precio
                    }
                })
                .ToListAsync();

            return solicitudesNoAceptadas;
        }

        //Obtenemos una solicitud pasando el vehiculoID:
        // GET: api/Solicitud/ByVehiculoID
        [HttpGet("ByVehiculoId/{vehiculoID}")]
        public async Task<ActionResult<SolicitudDTO>> GetSolicitudByVehiculoId(int vehiculoID)
        {
            var solicitud = await _context.Solicitudes
                .FirstOrDefaultAsync(s => s.VehiculoID == vehiculoID);

            if (solicitud == null)
            {
                return NotFound();
            }

            var solicitudDTO = new SolicitudDTO
            {
                SolicitudID = solicitud.SolicitudID,
                Estado = solicitud.Estado,
                TipoSolicitud = solicitud.TipoSolicitud,
                SucursalID = solicitud.SucursalID,
                VehiculoID = solicitud.VehiculoID,
                ClienteID = solicitud.ClienteID
            };

            return solicitudDTO;
        }

        //Método auxiliar para verificar si una solicitud existe en la base de datos
        private bool SolicitudExists(int id)
        {
            return _context.Solicitudes.Any(e => e.SolicitudID == id);
        }
    }
}
