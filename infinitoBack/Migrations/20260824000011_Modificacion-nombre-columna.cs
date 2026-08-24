using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infinitoBack.Migrations
{
    /// <inheritdoc />
    public partial class Modificacionnombrecolumna : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Excursiones");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Excursiones");

            migrationBuilder.DropColumn(
                name: "Seña",
                table: "Excursiones");

            migrationBuilder.RenameColumn(
                name: "DuracionDias",
                table: "Excursiones",
                newName: "CantDias");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Excursiones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CantDias",
                table: "Excursiones",
                newName: "DuracionDias");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Excursiones",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Excursiones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Excursiones",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Seña",
                table: "Excursiones",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
