using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SegundaAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenombreTextoObservaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Texto",
                table: "Proveedores",
                newName: "Observaciones");

            migrationBuilder.RenameColumn(
                name: "Texto",
                table: "FacturaVentaLinea",
                newName: "Observaciones");

            migrationBuilder.RenameColumn(
                name: "Texto",
                table: "FacturaVentaCabecera",
                newName: "Observaciones");

            migrationBuilder.RenameColumn(
                name: "Texto",
                table: "FacturaCompraLinea",
                newName: "Observaciones");

            migrationBuilder.RenameColumn(
                name: "Texto",
                table: "FacturaCompraCabecera",
                newName: "Observaciones");

            migrationBuilder.RenameColumn(
                name: "Texto",
                table: "Clientes",
                newName: "Observaciones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "Proveedores",
                newName: "Texto");

            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "FacturaVentaLinea",
                newName: "Texto");

            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "FacturaVentaCabecera",
                newName: "Texto");

            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "FacturaCompraLinea",
                newName: "Texto");

            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "FacturaCompraCabecera",
                newName: "Texto");

            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "Clientes",
                newName: "Texto");
        }
    }
}
