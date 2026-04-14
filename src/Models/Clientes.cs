using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    //Representa a la tabla Clientes en la base de datos.

    [Table("Clientes")]
    public class Clientes
    {
        [Key]
        [StringLength(25)]
        [Column("IDCliente")]
        public string IdCliente { get; set; } = string.Empty;

        [StringLength(300)]
        [Column("DescCliente")]
        public string DescCliente { get; set; } = string.Empty;

        [StringLength(300)]
        [Column("RazonSocial")]
        public string? RazonSocial { get; set; }
        
        [StringLength(25)]
        [Column("CifCliente")]
        public string CifCliente { get; set; } = string.Empty;

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
        [Column("CCCliente")]
        public string? CcCliente { get; set; }

        [Column("DtoComercial")]
        public decimal DtoComercial { get; set; }

        [Column("Observaciones", TypeName = "ntext")]
        public string? Observaciones { get; set; }

    }
}
