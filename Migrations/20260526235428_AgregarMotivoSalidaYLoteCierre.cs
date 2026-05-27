using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprendimientoApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMotivoSalidaYLoteCierre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MotivoSalida",
                table: "MovimientosStock",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Cerrado",
                table: "Lotes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MotivoCierre",
                table: "Lotes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoSalida",
                table: "MovimientosStock");

            migrationBuilder.DropColumn(
                name: "Cerrado",
                table: "Lotes");

            migrationBuilder.DropColumn(
                name: "MotivoCierre",
                table: "Lotes");
        }
    }
}
