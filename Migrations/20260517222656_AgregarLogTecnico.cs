using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprendimientoApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarLogTecnico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Ventas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Ventas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Productos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Productos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "MovimientosStock",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "MovimientosStock",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Ingredientes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Ingredientes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "MovimientosStock");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "MovimientosStock");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Ingredientes");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Ingredientes");
        }
    }
}
