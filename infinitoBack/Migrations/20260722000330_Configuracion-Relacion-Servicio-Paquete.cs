using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infinitoBack.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguracionRelacionServicioPaquete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "Paquetes");

            migrationBuilder.RenameColumn(
                name: "FechaInicio",
                table: "Paquetes",
                newName: "FechaSalida");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Paquetes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DuracionDias",
                table: "Paquetes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Seña",
                table: "Paquetes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Paquetes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaqueteServicio",
                columns: table => new
                {
                    ServiciosId = table.Column<int>(type: "int", nullable: false),
                    paquetesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaqueteServicio", x => new { x.ServiciosId, x.paquetesId });
                    table.ForeignKey(
                        name: "FK_PaqueteServicio_Paquetes_paquetesId",
                        column: x => x.paquetesId,
                        principalTable: "Paquetes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaqueteServicio_Servicios_ServiciosId",
                        column: x => x.ServiciosId,
                        principalTable: "Servicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaqueteServicio_paquetesId",
                table: "PaqueteServicio",
                column: "paquetesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaqueteServicio");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Paquetes");

            migrationBuilder.DropColumn(
                name: "DuracionDias",
                table: "Paquetes");

            migrationBuilder.DropColumn(
                name: "Seña",
                table: "Paquetes");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Paquetes");

            migrationBuilder.RenameColumn(
                name: "FechaSalida",
                table: "Paquetes",
                newName: "FechaInicio");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaFin",
                table: "Paquetes",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
