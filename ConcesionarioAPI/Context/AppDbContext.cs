using ConcesionarioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcesionarioAPI.Context
{
    public class AppDbContext : DbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasKey(u => u.UsuarioID);

            modelBuilder.Entity<Sucursal>()
                .HasKey(s => s.SucursalID);

            modelBuilder.Entity<Vehiculo>()
                .HasKey(v => v.VehiculoID);

            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.ClienteID);

            modelBuilder.Entity<Solicitud>()
                .HasKey(s => s.SolicitudID);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Sucursales)
                .WithOne(s => s.Usuario)
                .HasForeignKey(s => s.UsuarioID);

            modelBuilder.Entity<Sucursal>()
                .HasMany(s => s.Solicitudes)
                .WithOne(s => s.Sucursal)
                .HasForeignKey(s => s.SucursalID);

            modelBuilder.Entity<Solicitud>()
                .HasOne(s => s.Vehiculo)
                .WithMany(v => v.Solicitudes)
                .HasForeignKey(s => s.VehiculoID);

            modelBuilder.Entity<Solicitud>()
                .HasOne(s => s.Cliente)
                .WithMany(c => c.Solicitudes)
                .HasForeignKey(s => s.ClienteID);

            modelBuilder.Entity<Usuario>()
               .Property(u => u.NombreUsuario)
               .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.ClaveUsuario)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(b => b.TipoUsuario)
                .HasDefaultValue("normal");

            modelBuilder.Entity<Sucursal>()
                .HasIndex(s => s.UsuarioID);

            modelBuilder.Entity<Solicitud>()
                .HasIndex(s => s.SucursalID);

            modelBuilder.Entity<Solicitud>()
                .HasIndex(s => s.VehiculoID);

            modelBuilder.Entity<Solicitud>()
                .HasIndex(s => s.ClienteID);
        }
    }
}
