using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprendimientoApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCombos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Combos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComboOpciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComboId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsEleccion = table.Column<bool>(type: "bit", nullable: false),
                    ProductoFijoId = table.Column<int>(type: "int", nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboOpciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboOpciones_Combos_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboOpciones_Productos_ProductoFijoId",
                        column: x => x.ProductoFijoId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComboOpcionProductos",
                columns: table => new
                {
                    ComboOpcionId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboOpcionProductos", x => new { x.ComboOpcionId, x.ProductoId });
                    table.ForeignKey(
                        name: "FK_ComboOpcionProductos_ComboOpciones_ComboOpcionId",
                        column: x => x.ComboOpcionId,
                        principalTable: "ComboOpciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboOpcionProductos_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComboOpciones_ComboId",
                table: "ComboOpciones",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboOpciones_ProductoFijoId",
                table: "ComboOpciones",
                column: "ProductoFijoId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboOpcionProductos_ProductoId",
                table: "ComboOpcionProductos",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComboOpcionProductos");

            migrationBuilder.DropTable(
                name: "ComboOpciones");

            migrationBuilder.DropTable(
                name: "Combos");
        }
    }
}
