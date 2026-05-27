using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprendimientoApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrigenPedido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Idexterno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PedidoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    ComboId = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoItems_Combos_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PedidoItems_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItems_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PedidoItemOpciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoItemId = table.Column<int>(type: "int", nullable: false),
                    ComboOpcionId = table.Column<int>(type: "int", nullable: false),
                    ProductoElegidoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItemOpciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoItemOpciones_ComboOpciones_ComboOpcionId",
                        column: x => x.ComboOpcionId,
                        principalTable: "ComboOpciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItemOpciones_PedidoItems_PedidoItemId",
                        column: x => x.PedidoItemId,
                        principalTable: "PedidoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItemOpciones_Productos_ProductoElegidoId",
                        column: x => x.ProductoElegidoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItemOpciones_ComboOpcionId",
                table: "PedidoItemOpciones",
                column: "ComboOpcionId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItemOpciones_PedidoItemId",
                table: "PedidoItemOpciones",
                column: "PedidoItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItemOpciones_ProductoElegidoId",
                table: "PedidoItemOpciones",
                column: "ProductoElegidoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_ComboId",
                table: "PedidoItems",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_PedidoId",
                table: "PedidoItems",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_ProductoId",
                table: "PedidoItems",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoItemOpciones");

            migrationBuilder.DropTable(
                name: "PedidoItems");

            migrationBuilder.DropTable(
                name: "Pedidos");
        }
    }
}
