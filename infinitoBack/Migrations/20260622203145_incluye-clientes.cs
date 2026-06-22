using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infinitoBack.Migrations
{
    /// <inheritdoc />
    public partial class incluyeclientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CiCliente = table.Column<int>(type: "int", nullable: false),
                    FechaNacCliente = table.Column<DateOnly>(type: "date", nullable: false),
                    TelCliente = table.Column<int>(type: "int", nullable: false),
                    SaldoCliente = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
