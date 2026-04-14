using System.ComponentModel.DataAnnotations;

namespace src.DTOs.Articulos
{
    // Datos de entrada para crear o modificar un artículo
    // No hay campos calculados por código
    public class ArticuloCreateDto
    {
        [Display(Name = "descripción")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La {0} es obligatoria")]
        [StringLength(300, ErrorMessage = "La {0} no debe superar los {1} caracteres")]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string DescArticulo { get; set; } = string.Empty;

        [Display(Name = "estado")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El {0} es obligatorio")]
        [StringLength(10, ErrorMessage = "El {0} no debe superar los {1} caracteres")]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string Estado { get; set; } = string.Empty;

        [Display(Name = "familia")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El {0} es obligatorio")]
        [StringLength(10, ErrorMessage = "La {0} no debe superar los {1} caracteres")]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string Familia { get; set; } = string.Empty;

        [StringLength(10)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? CCVenta { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? CCCompra { get; set; }

        [Display(Name = "IVA")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El {0} es obligatorio")]
        [StringLength(10)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string TipoIva { get; set; } = string.Empty;

        [StringLength(10)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? UdVenta { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? UdCompra { get; set; }

        [Display(Name = "peso neto")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El {0} debe ser mayor que 0")]
        public decimal PesoNeto { get; set; }

        [Display(Name = "peso bruto")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El {0} debe ser mayor que 0")]
        public decimal PesoBruto { get; set; }

        [Display(Name = "plazo")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El {0} debe ser mayor que 0")]
        public decimal Plazo { get; set; }

        [Display(Name = "volumen")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El {0} debe ser mayor que 0")]
        public decimal Volumen { get; set; }
    }
}
