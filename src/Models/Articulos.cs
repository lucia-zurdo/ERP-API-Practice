using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    // Representa a la tabla Artículos en la base de datos.

    [Table("Articulos")]
    public class Articulos
    {
        [Key]
        [StringLength(25)]
        [Column("IDArticulo")]
        public string IdArticulo { get; set; } = string.Empty;

        [StringLength(300)]
        [Column("DescArticulo")]
        public string DescArticulo { get; set; } = string.Empty;

        [StringLength(10)]
        [Column("Estado")]
        public string Estado { get; set; } = string.Empty;

        [StringLength(10)]
        [Column("Familia")]
        public string Familia { get; set; } = string.Empty;

        [StringLength(10)]
        [Column("CCVenta")]
        public string? CCVenta { get; set; }

        [StringLength(10)]
        [Column("CCCompra")]
        public string? CCCompra { get; set; }

        [StringLength(10)]
        [Column("TipoIva")]
        public string? TipoIva { get; set; }

        [StringLength(10)]
        [Column("UdVenta")]
        public string? UdVenta { get; set; }

        [StringLength(10)]
        [Column("UdCompra")]
        public string? UdCompra { get; set; }

        [Column("PesoNeto")]
        public decimal PesoNeto { get; set; }

        [Column("PesoBruto")]
        public decimal PesoBruto { get; set; }

        [Column("Plazo")]
        public decimal Plazo { get; set; }

        [Column("Volumen")]
        public decimal Volumen { get; set; }
    }
}
