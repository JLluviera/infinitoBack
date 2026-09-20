using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infinitoBack.Migrations
{
    /// <inheritdoc />
    public partial class relacionestransaccionreservayclientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Clientes_ClienteId",
                table: "Transacciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Reservas_ReservaId",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_ClienteId",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_ReservaId",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "ReservaId",
                table: "Transacciones");

            migrationBuilder.RenameColumn(
                name: "FormaDePAgo",
                table: "Transacciones",
                newName: "FormaDePago");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_IdCliente",
                table: "Transacciones",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_IdReserva",
                table: "Transacciones",
                column: "IdReserva");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Clientes_IdCliente",
                table: "Transacciones",
                column: "IdCliente",
                principalTable: "Clientes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Reservas_IdReserva",
                table: "Transacciones",
                column: "IdReserva",
                principalTable: "Reservas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Clientes_IdCliente",
                table: "Transacciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Reservas_IdReserva",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_IdCliente",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_IdReserva",
                table: "Transacciones");

            migrationBuilder.RenameColumn(
                name: "FormaDePago",
                table: "Transacciones",
                newName: "FormaDePAgo");

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Transacciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReservaId",
                table: "Transacciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_ClienteId",
                table: "Transacciones",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_ReservaId",
                table: "Transacciones",
                column: "ReservaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Clientes_ClienteId",
                table: "Transacciones",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Reservas_ReservaId",
                table: "Transacciones",
                column: "ReservaId",
                principalTable: "Reservas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
