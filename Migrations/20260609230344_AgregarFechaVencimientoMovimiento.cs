using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprendimientoApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFechaVencimientoMovimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaVencimiento",
                table: "MovimientosStock",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovimientoAnuladoId",
                table: "MovimientosStock",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_MovimientoAnuladoId",
                table: "MovimientosStock",
                column: "MovimientoAnuladoId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_MovimientosStock_MovimientoAnuladoId",
                table: "MovimientosStock",
                column: "MovimientoAnuladoId",
                principalTable: "MovimientosStock",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosStock_MovimientosStock_MovimientoAnuladoId",
                table: "MovimientosStock");

            migrationBuilder.DropIndex(
                name: "IX_MovimientosStock_MovimientoAnuladoId",
                table: "MovimientosStock");

            migrationBuilder.DropColumn(
                name: "FechaVencimiento",
                table: "MovimientosStock");

            migrationBuilder.DropColumn(
                name: "MovimientoAnuladoId",
                table: "MovimientosStock");
        }
    }
}
