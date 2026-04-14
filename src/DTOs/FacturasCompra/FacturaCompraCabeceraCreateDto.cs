using System.ComponentModel.DataAnnotations;

namespace src.DTOs.FacturasCompra
{
    /*Datos de entrada para crear una factura de compra
     * Campos que el cliente NO debe enviar porque se gestionan automáticamente:
     *  - Importes (ImpLineas, ImpDtoFactura, BaseImponible, ImpIva, ImpTotal, etc.)
     *      → los calcula FacturaCalculoService antes de insertar en BD
     *  - IdFactura → autonumérico generado por SQL Server
     *  - CifProveedor y RazonSocial → se copian del registro del proveedor
     *      en el momento de crear la factura como dato histórico*/
    public class FacturaCompraCabeceraCreateDto
    {
        [Display(Name = "número de factura")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El {0} es obligatorio")]
        [StringLength(25, ErrorMessage = "El {0} no debe superar los {1} caracteres")]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string NFactura { get; set; } = string.Empty;

        public DateTime? FechaFactura { get; set; }

        [Display(Name = "Id del proveedor")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El {0} es obligatorio")]
        [StringLength(25, ErrorMessage = "El {0} no debe superar los {1} caracteres")]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string IdProveedor { get; set; } = string.Empty;

        [StringLength(10)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? FormaPago { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El estado es obligatorio")]
        public int Estado { get; set; }

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
        public decimal DtoFactura { get; set; }

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
        public decimal DtoProntoPago { get; set; }

        public decimal RecFinan { get; set; } = 0;

        public decimal RetencionIRPF { get; set; } = 0;

        public DateTime? FechaVencimiento { get; set; }

        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? Observaciones { get; set; }

        [Display(Name = "factura")]
        [Required(ErrorMessage = "La {0} debe tener al menos una línea")]
        [MinLength(1, ErrorMessage = "La {0} debe tener al menos una línea")]
        public List<FacturaCompraLineaCreateDto> Lineas { get; set; } = new();
    }
}
