using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infinitoBack.Migrations
{
    /// <inheritdoc />
    public partial class AjustarRelacionesReservas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AsignacionesAsientos_Excursiones_ExcursionId1",
                table: "AsignacionesAsientos");

            migrationBuilder.DropIndex(
                name: "IX_AsignacionesAsientos_ExcursionId1",
                table: "AsignacionesAsientos");

            migrationBuilder.DropColumn(
                name: "ExcursionId1",
                table: "AsignacionesAsientos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<int>(
                name: "ExcursionId1",
                table: "AsignacionesAsientos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesAsientos_ExcursionId1",
                table: "AsignacionesAsientos",
                column: "ExcursionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AsignacionesAsientos_Excursiones_ExcursionId1",
                table: "AsignacionesAsientos",
                column: "ExcursionId1",
                principalTable: "Excursiones",
                principalColumn: "Id");
        }
    }
}
