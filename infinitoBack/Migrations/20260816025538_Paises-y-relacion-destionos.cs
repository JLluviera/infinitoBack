using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infinitoBack.Migrations
{
    /// <inheritdoc />
    public partial class Paisesyrelaciondestionos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pais",
                table: "Destinos");

            migrationBuilder.AddColumn<int>(
                name: "IdPais",
                table: "Destinos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoPais = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinos_IdPais",
                table: "Destinos",
                column: "IdPais");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinos_Paises_IdPais",
                table: "Destinos",
                column: "IdPais",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinos_Paises_IdPais",
                table: "Destinos");

            migrationBuilder.DropTable(
                name: "Paises");

            migrationBuilder.DropIndex(
                name: "IX_Destinos_IdPais",
                table: "Destinos");

            migrationBuilder.DropColumn(
                name: "IdPais",
                table: "Destinos");

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "Destinos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
