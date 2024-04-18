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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConcesionarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
            // Crea un nuevo usuario con los datos proporcionados
            var usuario = new Usuario
            {
                NombreUsuario = usuarioDTO.NombreUsuario,
                ClaveUsuario = usuarioDTO.ClaveUsuario
            };

            // Asigna TipoUsuario solo si tiene un valor
            if (!string.IsNullOrEmpty(usuarioDTO.TipoUsuario))
            {
                usuario.TipoUsuario = usuarioDTO.TipoUsuario;
            }

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

        // POST para el login:
        [HttpPost("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] UsuarioDTO usuarioDTO)
        {
            var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.NombreUsuario == usuarioDTO.NombreUsuario && u.ClaveUsuario == usuarioDTO.ClaveUsuario);

            if (usuario == null)
                return NotFound(new { message = "Nombre de usuario o contraseña incorrectos" });

            var token = GenerateJwtToken(usuario);

            return new
            {
                usuario.UsuarioID,
                usuario.NombreUsuario,
                usuario.TipoUsuario,
                Token = token
            };
        }

        //Método que genera un Token para un usuario UNA VEZ SE HA LOGUEADO:
        private string GenerateJwtToken(Usuario user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UsuarioID.ToString()),
                new Claim(ClaimTypes.Name, user.NombreUsuario),
                new Claim(ClaimTypes.Role, user.TipoUsuario)
            };

            // Genera una clave secreta de 256 bits (32 bytes)
            var key = GenerateRandomKey(32);
            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // Método para generar una clave aleatoria
        private static string GenerateRandomKey(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(chars);
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
