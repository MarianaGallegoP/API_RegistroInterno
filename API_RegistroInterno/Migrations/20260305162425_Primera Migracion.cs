using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_RegistroInterno.Migrations
{
    /// <inheritdoc />
    public partial class PrimeraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "tblUsuariosClientesExternos",
                schema: "dbo",
                columns: table => new
                {
                    intId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vchTipoDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vchDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dtmExpidicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vchNombre1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vchNombre2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vchApellido1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vchApellido2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vchNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vchEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vchCelular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vchClave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    intIntentosFallidos = table.Column<int>(type: "int", nullable: false),
                    dtmUltimoAcceso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    intEstado = table.Column<int>(type: "int", nullable: false),
                    dtmCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dtmActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUsuariosClientesExternos", x => x.intId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblUsuariosClientesExternos",
                schema: "dbo");
        }
    }
}
