using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprendimientoApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCostoProduccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostoProduccion",
                table: "Productos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostoProduccion",
                table: "Productos");
        }
    }
}
