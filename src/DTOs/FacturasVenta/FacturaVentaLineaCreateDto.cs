using System.ComponentModel.DataAnnotations;

namespace src.DTOs.FacturasVenta
{
    /*Datos de entrada de una línea de factura de venta
     * El Importe no se incluye porque se calcula automáticamente:
     * Importe = Cantidad × Precio × (1 - Dto/100) × (1 - DtoProntoPago/100)*/
    public class FacturaVentaLineaCreateDto
    {
        public int IdLineaFactura { get; set; }

        [StringLength(25)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? IdArticulo { get; set; }

        [StringLength(300)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? DescArticulo { get; set; }

        [Display(Name = "IVA")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El {0} es obligatorio")]
        [StringLength(10)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string TipoIva { get; set; } = string.Empty;

        [Display(Name = "unidad de medida")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La {0} es obligatoria")]
        [StringLength(10, ErrorMessage = "La {0} no debe superar los {1} caracteres")]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string IdUdMedida { get; set; } = string.Empty;

        [Display(Name = "cantidad")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La {0} es obligatoria")]
        [Range(0.00000001, double.MaxValue, ErrorMessage = "La {0} debe ser mayor que 0")]
        public decimal Cantidad { get; set; }

        [Display(Name = "precio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El {0} es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El {0} no puede ser negativo. El {0} debe contener números")]
        public decimal Precio { get; set; }

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
        public decimal Dto { get; set; }

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
        public decimal DtoProntoPago { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? Lote { get; set; }

        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? Observaciones { get; set; }
    }
}
