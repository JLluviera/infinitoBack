using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infinitoBack.Migrations
{
    /// <inheritdoc />
    public partial class reorganizacionmodelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paquetes_Excursiones_IdExcursion",
                table: "Paquetes");

            migrationBuilder.RenameColumn(
                name: "IdExcursion",
                table: "Paquetes",
                newName: "IdDestino");

            migrationBuilder.RenameIndex(
                name: "IX_Paquetes_IdExcursion",
                table: "Paquetes",
                newName: "IX_Paquetes_IdDestino");

            migrationBuilder.AddForeignKey(
                name: "FK_Paquetes_Destinos_IdDestino",
                table: "Paquetes",
                column: "IdDestino",
                principalTable: "Destinos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paquetes_Destinos_IdDestino",
                table: "Paquetes");

            migrationBuilder.RenameColumn(
                name: "IdDestino",
                table: "Paquetes",
                newName: "IdExcursion");

            migrationBuilder.RenameIndex(
                name: "IX_Paquetes_IdDestino",
                table: "Paquetes",
                newName: "IX_Paquetes_IdExcursion");

            migrationBuilder.AddForeignKey(
                name: "FK_Paquetes_Excursiones_IdExcursion",
                table: "Paquetes",
                column: "IdExcursion",
                principalTable: "Excursiones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
