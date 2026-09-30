using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infinitoBack.Migrations
{
    /// <inheritdoc />
    public partial class PlantillaVehiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservaCliente_Clientes_ClienteId",
                table: "ReservaCliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Clientes_IdClientePagador",
                table: "Reservas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservaCliente",
                table: "ReservaCliente");

            migrationBuilder.DropIndex(
                name: "IX_ReservaCliente_ReservaId",
                table: "ReservaCliente");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ReservaCliente",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AsignacionAsientoId",
                table: "ReservaCliente",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlantillaVehiculoId",
                table: "Excursiones",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservaCliente",
                table: "ReservaCliente",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PlantillasVehiculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePlantilla = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPisos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantillasVehiculos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Asientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantillaVehiculoId = table.Column<int>(type: "int", nullable: false),
                    NumeroAsiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PisoAsiento = table.Column<int>(type: "int", nullable: false),
                    Fila = table.Column<int>(type: "int", nullable: false),
                    Columna = table.Column<int>(type: "int", nullable: false),
                    TipoAsiento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asientos_PlantillasVehiculos_PlantillaVehiculoId",
                        column: x => x.PlantillaVehiculoId,
                        principalTable: "PlantillasVehiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsignacionesAsientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExcursionId = table.Column<int>(type: "int", nullable: false),
                    AsientoId = table.Column<int>(type: "int", nullable: false),
                    ReservaClienteId = table.Column<int>(type: "int", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExcursionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionesAsientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsignacionesAsientos_Asientos_AsientoId",
                        column: x => x.AsientoId,
                        principalTable: "Asientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignacionesAsientos_Excursiones_ExcursionId",
                        column: x => x.ExcursionId,
                        principalTable: "Excursiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignacionesAsientos_Excursiones_ExcursionId1",
                        column: x => x.ExcursionId1,
                        principalTable: "Excursiones",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AsignacionesAsientos_ReservaCliente_ReservaClienteId",
                        column: x => x.ReservaClienteId,
                        principalTable: "ReservaCliente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservaCliente_ClienteId",
                table: "ReservaCliente",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaCliente_ReservaId_ClienteId",
                table: "ReservaCliente",
                columns: new[] { "ReservaId", "ClienteId" });

            migrationBuilder.CreateIndex(
                name: "IX_Excursiones_PlantillaVehiculoId",
                table: "Excursiones",
                column: "PlantillaVehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Asientos_PlantillaVehiculoId_PisoAsiento_Fila_Columna",
                table: "Asientos",
                columns: new[] { "PlantillaVehiculoId", "PisoAsiento", "Fila", "Columna" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesAsientos_AsientoId",
                table: "AsignacionesAsientos",
                column: "AsientoId");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesAsientos_ExcursionId_AsientoId",
                table: "AsignacionesAsientos",
                columns: new[] { "ExcursionId", "AsientoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesAsientos_ExcursionId1",
                table: "AsignacionesAsientos",
                column: "ExcursionId1");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesAsientos_ReservaClienteId",
                table: "AsignacionesAsientos",
                column: "ReservaClienteId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Excursiones_PlantillasVehiculos_PlantillaVehiculoId",
                table: "Excursiones",
                column: "PlantillaVehiculoId",
                principalTable: "PlantillasVehiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaCliente_Clientes_ClienteId",
                table: "ReservaCliente",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Clientes_IdClientePagador",
                table: "Reservas",
                column: "IdClientePagador",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Excursiones_PlantillasVehiculos_PlantillaVehiculoId",
                table: "Excursiones");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservaCliente_Clientes_ClienteId",
                table: "ReservaCliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Clientes_IdClientePagador",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "AsignacionesAsientos");

            migrationBuilder.DropTable(
                name: "Asientos");

            migrationBuilder.DropTable(
                name: "PlantillasVehiculos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservaCliente",
                table: "ReservaCliente");

            migrationBuilder.DropIndex(
                name: "IX_ReservaCliente_ClienteId",
                table: "ReservaCliente");

            migrationBuilder.DropIndex(
                name: "IX_ReservaCliente_ReservaId_ClienteId",
                table: "ReservaCliente");

            migrationBuilder.DropIndex(
                name: "IX_Excursiones_PlantillaVehiculoId",
                table: "Excursiones");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ReservaCliente");

            migrationBuilder.DropColumn(
                name: "AsignacionAsientoId",
                table: "ReservaCliente");

            migrationBuilder.DropColumn(
                name: "PlantillaVehiculoId",
                table: "Excursiones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservaCliente",
                table: "ReservaCliente",
                columns: new[] { "ClienteId", "ReservaId" });

            migrationBuilder.CreateIndex(
                name: "IX_ReservaCliente_ReservaId",
                table: "ReservaCliente",
                column: "ReservaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservaCliente_Clientes_ClienteId",
                table: "ReservaCliente",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Clientes_IdClientePagador",
                table: "Reservas",
                column: "IdClientePagador",
                principalTable: "Clientes",
                principalColumn: "Id");
        }
    }
}
