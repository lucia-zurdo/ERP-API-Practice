using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    // Representa a la tabla Factura Venta Cabecera en la base de datos.

    [Table("FacturaVentaCabecera")]
    public class FacturaVentaCabecera
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autonumérico
        [Column("IDFactura")]
        public int IdFactura { get; set; }

        [StringLength(25)]
        [Column("NFactura")]
        public string NFactura { get; set; } = string.Empty;

        [Column("FechaFactura")]
        public DateTime? FechaFactura { get; set; }

        [StringLength(25)]
        [Column("IDCliente")]
        public string IdCliente { get; set; } = string.Empty;

        [StringLength(50)]
        [Column("CifCliente")]
        public string? CifCliente { get; set; }

        [StringLength(300)]
        [Column("RazonSocial")]
        public string? RazonSocial { get; set; }

        [StringLength(10)]
        [Column("FormaPago")]
        public string? FormaPago { get; set; }

        [Column("Estado")]
        public int Estado { get; set; }

        [Column("ImpLineas")]
        public decimal ImpLineas { get; set; }

        [Column("DtoFactura")]
        public decimal DtoFactura { get; set; }

        [Column("ImpDtoFactura")]
        public decimal ImpDtoFactura { get; set; }

        [Column("DtoProntoPago")]
        public decimal DtoProntoPago { get; set; }

        [Column("ImpDpp")]
        public decimal ImpDpp { get; set; }

        [Column("RecFinan")]
        public decimal RecFinan { get; set; }

        [Column("ImpRecFinan")]
        public decimal ImpRecFinan { get; set; }

        [Column("ImpRE")]
        public decimal ImpRE { get; set; }

        [Column("BaseImponible")]
        public decimal BaseImponible { get; set; }

        [Column("ImpIva")]
        public decimal ImpIva { get; set; }

        [Column("RetencionIRPF")]
        public decimal RetencionIRPF { get; set; }

        [Column("ImpRetencion")]
        public decimal ImpRetencion { get; set; }

        [Column("ImpTotal")]
        public decimal ImpTotal { get; set; }

        [Column("FechaVencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        [Column("Observaciones", TypeName = "ntext")]
        public string? Observaciones { get; set; }

        public ICollection<FacturaVentaLinea> Lineas { get; set; } = new List<FacturaVentaLinea>();
    }
}
