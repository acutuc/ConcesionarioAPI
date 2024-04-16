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
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            // Obtén solo los datos del usuario sin incluir sucursales y vehículos
            var usuarios = await _context.Usuarios.Select(u => new UsuarioDTO
            {
                UsuarioID = u.UsuarioID,
                NombreUsuario = u.NombreUsuario,
                ClaveUsuario = u.ClaveUsuario,
                TipoUsuario = u.TipoUsuario
            }).ToListAsync();

            return usuarios;
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.UsuarioID == id)
                .Select(u => new UsuarioDTO
                {
                    UsuarioID = u.UsuarioID,
                    NombreUsuario = u.NombreUsuario,
                    ClaveUsuario = u.ClaveUsuario,
                    TipoUsuario = u.TipoUsuario
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/Usuario
        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario(UsuarioDTO usuarioDTO)
        {
            // Verifica si hay un UsuarioID proporcionado, y si es así, devuelve un error
            if (usuarioDTO.UsuarioID != 0)
            {
                ModelState.AddModelError("UsuarioID", "El UsuarioID no debe ser proporcionado al crear un nuevo usuario.");
                return BadRequest(ModelState);
            }

            // Crea un nuevo usuario con los datos proporcionados
            var usuario = new Usuario
            {
                NombreUsuario = usuarioDTO.NombreUsuario,
                ClaveUsuario = usuarioDTO.ClaveUsuario,
                TipoUsuario = usuarioDTO.TipoUsuario
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Devuelve la respuesta con el UsuarioDTO creado
            return CreatedAtAction("GetUsuario", new { id = usuario.UsuarioID }, new UsuarioDTO
            {
                UsuarioID = usuario.UsuarioID,
                NombreUsuario = usuario.NombreUsuario,
                ClaveUsuario = usuario.ClaveUsuario,
                TipoUsuario = usuario.TipoUsuario
            });
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDTO usuarioDTO)
        {
            if (id != usuarioDTO.UsuarioID)
            {
                return BadRequest();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos relevantes del usuario
            usuario.NombreUsuario = usuarioDTO.NombreUsuario;
            usuario.ClaveUsuario = usuarioDTO.ClaveUsuario;
            usuario.TipoUsuario = usuarioDTO.TipoUsuario;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioID == id);
        }
    }
}
