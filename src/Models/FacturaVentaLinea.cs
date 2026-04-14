using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    // Representa a la tabla Factura Venta Línea en la base de datos.

    [Table("FacturaVentaLinea")]
    public class FacturaVentaLinea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autonumérico
        [Column("IDLineaFactura")]
        public int IdLineaFactura { get; set; }

        [Column("IDFactura")]
        public int IdFactura { get; set; }

        [StringLength(25)]
        [Column("IDArticulo")]
        public string? IdArticulo { get; set; }

        [StringLength(300)]
        [Column("DescArticulo")]
        public string? DescArticulo { get; set; }

        [StringLength(10)]
        [Column("TipoIVA")]
        public string? TipoIva { get; set; }

        [StringLength(10)]
        [Column("IDUdMedida")]
        public string IdUdMedida { get; set; } = string.Empty;

        [Column("Cantidad")]
        public decimal Cantidad { get; set; }

        [Column("Precio")]
        public decimal Precio { get; set; }

        [Column("Dto")]
        public decimal Dto { get; set; }

        [Column("DtoProntoPago")]
        public decimal DtoProntoPago { get; set; }

        [Column("Importe")]
        public decimal Importe { get; set; }

        [StringLength(50)]
        [Column("Lote")]
        public string? Lote { get; set; }

        [Column("Observaciones", TypeName = "ntext")]
        public string? Observaciones { get; set; }

        [ForeignKey("IDFactura")]
        public FacturaVentaCabecera Cabecera { get; set; } = null!;
    }
}
