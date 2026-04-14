using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    // Representa a la tabla Proveedores en la base de datos.

    [Table("Proveedores")]
    public class Proveedores
    {
        [Key]
        [StringLength(25)]
        [Column("IDProveedor")]
        public string IdProveedor { get; set; } = string.Empty;

        [StringLength(300)]
        [Column("DescProveedor")]
        public string? DescProveedor { get; set; }

        [StringLength(300)]
        [Column("RazonSocial")]
        public string? RazonSocial { get; set; }

        [StringLength(25)]
        [Column("CifProveedor")]
        public string CifProveedor { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("Direccion")]
        public string? Direccion { get; set; }

        [StringLength(25)]
        [Column("CodPostal")]
        public string CodPostal { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("Poblacion")]
        public string? Poblacion { get; set; }

        [StringLength(100)]
        [Column("Provincia")]
        public string? Provincia { get; set; }

        [StringLength(10)]
        [Column("Pais")]
        public string Pais { get; set; } = string.Empty;

        [StringLength(25)]
        [Column("Telefono")]
        public string? Telefono { get; set; }

        [StringLength(25)]
        [Column("Fax")]
        public string? Fax { get; set; }

        [StringLength(100)]
        [Column("Email")]
        public string? EMail { get; set; }

        [StringLength(100)]
        [Column("Web")]
        public string? Web { get; set; }

        [StringLength(25)]
        [Column("Telefono2")]
        public string? Telefono2 { get; set; }

        [StringLength(25)]
        [Column("Movil")]
        public string? Movil { get; set; }

        [StringLength(10)]
        [Column("FormaPago")]
        public string FormaPago { get; set; } = string.Empty;

        [StringLength(10)]
        [Column("CCProveedor")]
        public string? CcProveedor { get; set; }

        [Column("DtoComercial")]
        public decimal DtoComercial { get; set; }

        [Column("Observaciones", TypeName = "ntext")]
        public string? Observaciones { get; set; }
    }
}
