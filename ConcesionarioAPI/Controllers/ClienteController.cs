using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcesionarioAPI.Context;
using ConcesionarioAPI.Models;
using ConcesionarioAPI.DTOs;

namespace ConcesionarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .Select(c => new ClienteDTO
                {
                    ClienteID = c.ClienteID,
                    NombreCliente = c.NombreCliente,
                    ApellidosCliente = c.ApellidosCliente,
                    TelefonoCliente = c.TelefonoCliente,
                    DNI = c.DNI
                })
                .ToListAsync();

            return clientes;
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound($"No se encontró nigún cliente con el ID {id}.");
            }

            var clienteDTO = new ClienteDTO
            {
                ClienteID = cliente.ClienteID,
                NombreCliente = cliente.NombreCliente,
                ApellidosCliente = cliente.ApellidosCliente,
                TelefonoCliente = cliente.TelefonoCliente,
                DNI = cliente.DNI
            };

            return clienteDTO;
        }

        // POST: api/Cliente
        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> PostCliente(ClienteDTO clienteDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Controlamos que no hayan dos clientes con el mismo DNI en nuestra BD:
            if (_context.Clientes.Any(u => u.DNI == clienteDTO.DNI))
            {
                return BadRequest("Ya existe un cliente con el mismo DNI en la base de datos.");
            }

            var cliente = new Cliente
            {
                NombreCliente = clienteDTO.NombreCliente,
                ApellidosCliente = clienteDTO.ApellidosCliente,
                TelefonoCliente = clienteDTO.TelefonoCliente,
                DNI = clienteDTO.DNI
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            clienteDTO.ClienteID = cliente.ClienteID;

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteID }, clienteDTO);
        }

        // PUT: api/Cliente/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClienteDTO clienteDTO)
        {
            if (id != clienteDTO.ClienteID)
            {
                return BadRequest();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            cliente.NombreCliente = clienteDTO.NombreCliente;
            cliente.ApellidosCliente = clienteDTO.ApellidosCliente;
            cliente.TelefonoCliente = clienteDTO.TelefonoCliente;
            cliente.DNI = clienteDTO.DNI;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Devolvemos si existe o no un cliente mediante su DNI:
        [HttpGet("Existe/{dni}")]
        public async Task<ActionResult<bool>> ExisteCliente(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                return BadRequest("El DNI no puede estar vacío.");
            }

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.DNI == dni);
            return cliente != null;
        }

        //Nos devuelve cliente a través de su DNI:
        [HttpGet("PorDni/{dni}")]
        public async Task<ActionResult<ClienteDTO>> GetClientePorDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                return BadRequest("El DNI no puede estar vacío.");
            }

            var cliente = await _context.Clientes
                .Where(c => c.DNI == dni)
                .Select(c => new ClienteDTO
                {
                    ClienteID = c.ClienteID,
                    NombreCliente = c.NombreCliente,
                    ApellidosCliente = c.ApellidosCliente,
                    TelefonoCliente = c.TelefonoCliente,
                    DNI = c.DNI
                })
                .FirstOrDefaultAsync();

            if (cliente == null)
            {
                return NotFound($"El cliente con el DNI {dni} no existe.");
            }

            return cliente;
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteID == id);
        }
    }
}
