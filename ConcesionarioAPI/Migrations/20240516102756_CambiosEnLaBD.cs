using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcesionarioAPI.Migrations
{
    /// <inheritdoc />
    public partial class CambiosEnLaBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehiculos_Sucursales_SucursalID",
                table: "Vehiculos");

            migrationBuilder.DropIndex(
                name: "IX_Vehiculos_SucursalID",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "SucursalID",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "Vendido",
                table: "Vehiculos");

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidosCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonoCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DNI = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteID);
                });

            migrationBuilder.CreateTable(
                name: "Solicitudes",
                columns: table => new
                {
                    SolicitudID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoSolicitud = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SucursalID = table.Column<int>(type: "int", nullable: false),
                    VehiculoID = table.Column<int>(type: "int", nullable: false),
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes", x => x.SolicitudID);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Clientes_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Sucursales_SucursalID",
                        column: x => x.SucursalID,
                        principalTable: "Sucursales",
                        principalColumn: "SucursalID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Vehiculos_VehiculoID",
                        column: x => x.VehiculoID,
                        principalTable: "Vehiculos",
                        principalColumn: "VehiculoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_ClienteID",
                table: "Solicitudes",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_SucursalID",
                table: "Solicitudes",
                column: "SucursalID");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_VehiculoID",
                table: "Solicitudes",
                column: "VehiculoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Solicitudes");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.AddColumn<int>(
                name: "SucursalID",
                table: "Vehiculos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Vendido",
                table: "Vehiculos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_SucursalID",
                table: "Vehiculos",
                column: "SucursalID");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehiculos_Sucursales_SucursalID",
                table: "Vehiculos",
                column: "SucursalID",
                principalTable: "Sucursales",
                principalColumn: "SucursalID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
