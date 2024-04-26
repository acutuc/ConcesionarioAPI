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
using Microsoft.AspNetCore.Authorization;

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

        //Obtenemos todos los usuarios:
        // GET: api/Usuario
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            //Obtenemos solo los datos del usuario sin incluir sucursales y vehículos:
            var usuarios = await _context.Usuarios.Select(u => new UsuarioDTO
            {
                UsuarioID = u.UsuarioID,
                NombreUsuario = u.NombreUsuario,
                ClaveUsuario = u.ClaveUsuario,
                TipoUsuario = u.TipoUsuario
            }).ToListAsync();

            return usuarios;
        }

        //Obtenemos un usuario por ID:
        // GET: api/Usuario/5
        [Authorize]
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

        //Creamos un usuario:
        // POST: api/Usuario
        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario(UsuarioDTO usuarioDTO)
        {
            //Controlamos que usuario y clave no estén vacíos:
            if (string.IsNullOrEmpty(usuarioDTO.NombreUsuario) || string.IsNullOrEmpty(usuarioDTO.ClaveUsuario))
            {
                return BadRequest("El nombre de usuario y la contraseña son obligatorios.");
            }

            //Controlamos que no hayan dos usuarios con el mismo nombre en nuestra BD:
            if (_context.Usuarios.Any(u => u.NombreUsuario == usuarioDTO.NombreUsuario))
            {
                return BadRequest("Ya existe un usuario con el mismo nombre.");
            }

            //Creamos un nuevo usuario con los datos proporcionados:
            var usuario = new Usuario
            {
                NombreUsuario = usuarioDTO.NombreUsuario,
                ClaveUsuario = usuarioDTO.ClaveUsuario
            };

            //Asignamos TipoUsuario solo si tiene un valor:
            if (!string.IsNullOrEmpty(usuarioDTO.TipoUsuario))
            {
                usuario.TipoUsuario = usuarioDTO.TipoUsuario;
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            //Devolvemos la respuesta con el UsuarioDTO creado:
            return CreatedAtAction("GetUsuario", new { id = usuario.UsuarioID }, new UsuarioDTO
            {
                UsuarioID = usuario.UsuarioID,
                NombreUsuario = usuario.NombreUsuario,
                ClaveUsuario = usuario.ClaveUsuario,
                TipoUsuario = usuario.TipoUsuario
            });
        }

        //Servicio para loguearnos:
        // POST para el login:
        [HttpPost("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] UsuarioDTO usuarioDTO)
        {
            var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.NombreUsuario == usuarioDTO.NombreUsuario && u.ClaveUsuario == usuarioDTO.ClaveUsuario);

            if (usuario == null)
                return Unauthorized(new { message = "Nombre de usuario o contraseña incorrectos" });

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
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UsuarioID.ToString()),
                new Claim(ClaimTypes.Name, user.NombreUsuario),
                new Claim(ClaimTypes.Role, user.TipoUsuario)
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //Método para generar una clave aleatoria
        /*private static string GenerateRandomKey(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(chars);
        }*/

        //Editamos un usuario por ID:
        // PUT: api/Usuario/5
        [Authorize]
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

            //Actualizamos solo los campos relevantes del usuario:
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

        //Eliminamos un usuario por ID:
        // DELETE: api/Usuario/5
        [Authorize]
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

        //Obtenemos las sucursales de un usuario pasando su ID:
        // GET: api/Usuario/{id}/sucursales
        [Authorize]
        [HttpGet("{id}/sucursales")]
        public async Task<ActionResult<IEnumerable<SucursalDTO>>> GetSucursalesByUsuario(int id)
        {
            var sucursales = await _context.Sucursales
                .Where(s => s.UsuarioID == id)
                .Select(s => new SucursalDTO
                {
                    SucursalID = s.SucursalID,
                    NombreSucursal = s.NombreSucursal,
                    Ubicacion = s.Ubicacion
                })
                .ToListAsync();

            if (sucursales == null || !sucursales.Any())
            {
                return NotFound("No se encontraron sucursales para el usuario especificado.");
            }

            return sucursales;
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioID == id);
        }
    }
}
