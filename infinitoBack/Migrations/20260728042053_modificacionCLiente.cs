using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infinitoBack.Migrations
{
    /// <inheritdoc />
    public partial class modificacionCLiente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CiCliente",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "SaldoCliente",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "TelCliente",
                table: "Clientes",
                newName: "Ci");

            migrationBuilder.RenameColumn(
                name: "NombreCliente",
                table: "Clientes",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "FechaNacCliente",
                table: "Clientes",
                newName: "FechaNacimiento");

            migrationBuilder.RenameColumn(
                name: "ApellidoCliente",
                table: "Clientes",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "IdCliente",
                table: "Clientes",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Saldo",
                table: "Clientes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Saldo",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "Clientes",
                newName: "NombreCliente");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Clientes",
                newName: "ApellidoCliente");

            migrationBuilder.RenameColumn(
                name: "FechaNacimiento",
                table: "Clientes",
                newName: "FechaNacCliente");

            migrationBuilder.RenameColumn(
                name: "Ci",
                table: "Clientes",
                newName: "TelCliente");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Clientes",
                newName: "IdCliente");

            migrationBuilder.AddColumn<int>(
                name: "CiCliente",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "SaldoCliente",
                table: "Clientes",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
