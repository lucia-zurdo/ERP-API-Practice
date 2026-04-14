using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SegundaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articulos",
                columns: table => new
                {
                    IDArticulo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DescArticulo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Familia = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CCVenta = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CCCompra = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TipoIva = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UdVenta = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UdCompra = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PesoNeto = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    PesoBruto = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Plazo = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Volumen = table.Column<decimal>(type: "numeric(23,8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articulos", x => x.IDArticulo);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IDCliente = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DescCliente = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CifCliente = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CodPostal = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Poblacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Provincia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Web = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefono2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Movil = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    FormaPago = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CCCliente = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DtoComercial = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Texto = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IDCliente);
                });

            migrationBuilder.CreateTable(
                name: "FacturaCompraCabecera",
                columns: table => new
                {
                    IDFactura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NFactura = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FechaFactura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IDProveedor = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CifProveedor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RazonSocial = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FormaPago = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    ImpLineas = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    DtoFactura = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpDtoFactura = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    DtoProntoPago = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpDpp = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    RecFinan = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpRecFinan = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpRE = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    BaseImponible = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpIva = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    RetencionIRPF = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpRetencion = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpTotal = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Texto = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturaCompraCabecera", x => x.IDFactura);
                });

            migrationBuilder.CreateTable(
                name: "FacturaVentaCabecera",
                columns: table => new
                {
                    IDFactura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NFactura = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FechaFactura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IDCliente = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CifCliente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RazonSocial = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FormaPago = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    ImpLineas = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    DtoFactura = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpDtoFactura = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    DtoProntoPago = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpDpp = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    RecFinan = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpRecFinan = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpRE = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    BaseImponible = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpIva = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    RetencionIRPF = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpRetencion = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    ImpTotal = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Texto = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturaVentaCabecera", x => x.IDFactura);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    IDProveedor = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DescProveedor = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RazonSocial = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CifProveedor = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CodPostal = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Poblacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Provincia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Web = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefono2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Movil = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    FormaPago = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CCProveedor = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DtoComercial = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Texto = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.IDProveedor);
                });

            migrationBuilder.CreateTable(
                name: "FacturaCompraLinea",
                columns: table => new
                {
                    IDLineaFactura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDFactura = table.Column<int>(type: "int", nullable: false),
                    IDArticulo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DescArticulo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TipoIVA = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IDUdMedida = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Precio = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Dto = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    DtoProntoPago = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Importe = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Lote = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Texto = table.Column<string>(type: "ntext", nullable: true),
                    IDFactura1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturaCompraLinea", x => x.IDLineaFactura);
                    table.ForeignKey(
                        name: "FK_FacturaCompraLinea_FacturaCompraCabecera_IDFactura",
                        column: x => x.IDFactura,
                        principalTable: "FacturaCompraCabecera",
                        principalColumn: "IDFactura",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacturaVentaLinea",
                columns: table => new
                {
                    IDLineaFactura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDFactura = table.Column<int>(type: "int", nullable: false),
                    IDArticulo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DescArticulo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TipoIVA = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IDUdMedida = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Precio = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Dto = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    DtoProntoPago = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Importe = table.Column<decimal>(type: "numeric(23,8)", nullable: false),
                    Lote = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Texto = table.Column<string>(type: "ntext", nullable: true),
                    IDFactura1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturaVentaLinea", x => x.IDLineaFactura);
                    table.ForeignKey(
                        name: "FK_FacturaVentaLinea_FacturaVentaCabecera_IDFactura",
                        column: x => x.IDFactura,
                        principalTable: "FacturaVentaCabecera",
                        principalColumn: "IDFactura",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacturaCompraLinea_IDFactura",
                table: "FacturaCompraLinea",
                column: "IDFactura");

            migrationBuilder.CreateIndex(
                name: "IX_FacturaVentaLinea_IDFactura",
                table: "FacturaVentaLinea",
                column: "IDFactura");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articulos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "FacturaCompraLinea");

            migrationBuilder.DropTable(
                name: "FacturaVentaLinea");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "FacturaCompraCabecera");

            migrationBuilder.DropTable(
                name: "FacturaVentaCabecera");
        }
    }
}
