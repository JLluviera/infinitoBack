using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infinitoBack.Migrations
{
    /// <inheritdoc />
    public partial class ClienteReservas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Saldo",
                table: "Clientes");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaVencimientoCi",
                table: "Clientes",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaReserva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoReserva = table.Column<int>(type: "int", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdClientePagador = table.Column<int>(type: "int", nullable: false),
                    IdExcursion = table.Column<int>(type: "int", nullable: false),
                    IdPaquete = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservas_Clientes_IdClientePagador",
                        column: x => x.IdClientePagador,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservas_Excursiones_IdExcursion",
                        column: x => x.IdExcursion,
                        principalTable: "Excursiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservas_Paquetes_IdPaquete",
                        column: x => x.IdPaquete,
                        principalTable: "Paquetes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservaCliente",
                columns: table => new
                {
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    ReservaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaCliente", x => new { x.ClienteId, x.ReservaId });
                    table.ForeignKey(
                        name: "FK_ReservaCliente_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaCliente_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservaCliente_ReservaId",
                table: "ReservaCliente",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdClientePagador",
                table: "Reservas",
                column: "IdClientePagador");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdExcursion",
                table: "Reservas",
                column: "IdExcursion");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdPaquete",
                table: "Reservas",
                column: "IdPaquete");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservaCliente");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropColumn(
                name: "FechaVencimientoCi",
                table: "Clientes");

            migrationBuilder.AddColumn<decimal>(
                name: "Saldo",
                table: "Clientes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
