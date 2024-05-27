﻿// <auto-generated />
using ConcesionarioAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConcesionarioAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ConcesionarioAPI.Models.Cliente", b =>
                {
                    b.Property<int>("ClienteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClienteID"));

                    b.Property<string>("ApellidosCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DNI")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelefonoCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClienteID");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("ConcesionarioAPI.Models.Solicitud", b =>
                {
                    b.Property<int>("SolicitudID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SolicitudID"));

                    b.Property<int>("ClienteID")
                        .HasColumnType("int");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SucursalID")
                        .HasColumnType("int");

                    b.Property<string>("TipoSolicitud")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehiculoID")
                        .HasColumnType("int");

                    b.HasKey("SolicitudID");

                    b.HasIndex("ClienteID");

                    b.HasIndex("SucursalID");

                    b.HasIndex("VehiculoID");

                    b.ToTable("Solicitudes");
                });

            modelBuilder.Entity("ConcesionarioAPI.Models.Sucursal", b =>
                {
                    b.Property<int>("SucursalID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SucursalID"));

                    b.Property<string>("NombreSucursal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ubicacion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioID")
                        .HasColumnType("int");

                    b.HasKey("SucursalID");

                    b.HasIndex("UsuarioID");

                    b.ToTable("Sucursales");
                });

            modelBuilder.Entity("ConcesionarioAPI.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsuarioID"));

                    b.Property<string>("ClaveUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("normal");

                    b.HasKey("UsuarioID");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ConcesionarioAPI.Models.Vehiculo", b =>
                {
                    b.Property<int>("VehiculoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VehiculoID"));

                    b.Property<int>("Anio")
                        .HasColumnType("int");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Precio")
                        .HasColumnType("float");

                    b.HasKey("VehiculoID");

                    b.ToTable("Vehiculos");
                });

            modelBuilder.Entity("ConcesionarioAPI.Models.Solicitud", b =>
                {
                    b.HasOne("ConcesionarioAPI.Models.Cliente", "Cliente")
                        .WithMany("Solicitudes")
                        .HasForeignKey("ClienteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConcesionarioAPI.Models.Sucursal", "Sucursal")
                        .WithMany("Solicitudes")
                        .HasForeignKey("SucursalID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConcesionarioAPI.Models.Vehiculo", "Vehiculo")
                        .WithMany("Solicitudes")
                        .HasForeignKey("VehiculoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Sucursal");

                    b.Navigation("Vehiculo");
                });

            modelBuilder.Entity("ConcesionarioAPI.Models.Sucursal", b =>
                {
                    b.HasOne("ConcesionarioAPI.Models.Usuario", "Usuario")
                        .WithMany("Sucursales")
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ConcesionarioAPI.Models.Cliente", b =>
                {
                    b.Navigation("Solicitudes");
                });

            modelBuilder.Entity("ConcesionarioAPI.Models.Sucursal", b =>
                {
                    b.Navigation("Solicitudes");
                });

            modelBuilder.Entity("ConcesionarioAPI.Models.Usuario", b =>
                {
                    b.Navigation("Sucursales");
                });

            modelBuilder.Entity("ConcesionarioAPI.Models.Vehiculo", b =>
                {
                    b.Navigation("Solicitudes");
                });
#pragma warning restore 612, 618
        }
    }
}
