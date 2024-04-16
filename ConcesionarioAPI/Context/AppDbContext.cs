using ConcesionarioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcesionarioAPI.Context
{
    public class AppDbContext : DbContext
    {
        //Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Sucursales)
                .WithOne(s => s.Usuario)
                .HasForeignKey(s => s.UsuarioID);

            modelBuilder.Entity<Sucursal>()
                .HasMany(s => s.Vehiculos)
                .WithOne(c => c.Sucursal)
                .HasForeignKey(c => c.SucursalID);
        }
    }
}
