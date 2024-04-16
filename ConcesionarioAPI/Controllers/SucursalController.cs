﻿using System;
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
    public class SucursalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SucursalController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Sucursal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SucursalDTO>>> GetSucursales()
        {
            var sucursales = await _context.Sucursales.Select(s => new SucursalDTO
            {
                SucursalID = s.SucursalID,
                NombreSucursal = s.NombreSucursal,
                Ubicacion = s.Ubicacion,
                UsuarioID = s.UsuarioID  // Incluye el UsuarioID en el mapeo
            }).ToListAsync();

            return sucursales;
        }

        // GET: api/Sucursal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SucursalDTO>> GetSucursal(int id)
        {
            var sucursal = await _context.Sucursales
                .Where(s => s.SucursalID == id)
                .Select(s => new SucursalDTO
                {
                    SucursalID = s.SucursalID,
                    NombreSucursal = s.NombreSucursal,
                    Ubicacion = s.Ubicacion
                })
                .FirstOrDefaultAsync();

            if (sucursal == null)
            {
                return NotFound();
            }

            return sucursal;
        }

        // POST: api/Sucursal
        [HttpPost]
        public async Task<ActionResult<SucursalDTO>> PostSucursal(SucursalDTO sucursalDTO)
        {
            // Verifica si el usuario existe
            var usuarioExistente = await _context.Usuarios.FindAsync(sucursalDTO.UsuarioID);
            if (usuarioExistente == null)
            {
                return BadRequest("El UsuarioID proporcionado no corresponde a un usuario existente.");
            }

            // Crea una nueva instancia de Sucursal con los datos proporcionados
            var sucursal = new Sucursal
            {
                NombreSucursal = sucursalDTO.NombreSucursal,
                Ubicacion = sucursalDTO.Ubicacion,
                UsuarioID = sucursalDTO.UsuarioID // Asigna el UsuarioID proporcionado
            };

            // Agrega la nueva sucursal al contexto y guarda los cambios en la base de datos
            _context.Sucursales.Add(sucursal);
            await _context.SaveChangesAsync();

            // Devuelve la sucursal creada como resultado
            return CreatedAtAction("GetSucursal", new { id = sucursal.SucursalID }, sucursalDTO);
        }

        // PUT: api/Sucursal/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSucursal(int id, SucursalDTO sucursalDTO)
        {
            if (id != sucursalDTO.SucursalID)
            {
                return BadRequest();
            }

            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal == null)
            {
                return NotFound();
            }

            sucursal.NombreSucursal = sucursalDTO.NombreSucursal;
            sucursal.Ubicacion = sucursalDTO.Ubicacion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SucursalExists(id))
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

        // DELETE: api/Sucursal/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSucursal(int id)
        {
            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal == null)
            {
                return NotFound();
            }

            _context.Sucursales.Remove(sucursal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SucursalExists(int id)
        {
            return _context.Sucursales.Any(e => e.SucursalID == id);
        }
    }
}
