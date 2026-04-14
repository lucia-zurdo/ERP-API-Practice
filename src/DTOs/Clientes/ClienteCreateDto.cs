using System.ComponentModel.DataAnnotations;

namespace src.DTOs.Clientes
{
    // Datos de entrada para crear o modificar un cliente
    // No hay campos calculados por código
    public class ClienteCreateDto
    {
        [Display(Name = "descripcion")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La {0} es obligatoria")]
        [StringLength(300, ErrorMessage = "La {0} no debe superar los {1} caracteres")]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string DescCliente { get; set; } = string.Empty;

        [Display(Name = "razón social")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La {0} es obligatoria")]
        [StringLength(300)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string RazonSocial { get; set; } = string.Empty;

        [Display(Name = "CIF")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El {0} es obligatorio")]
        [StringLength(25, ErrorMessage = "El {0} no debe superar los {1} caracteres")]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string CifCliente { get; set; } = string.Empty;

        [StringLength(100)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? Direccion { get; set; }

        [Display(Name = "Código Postal")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El {0} es obligatorio")]
        [StringLength(25, ErrorMessage = "El {0} no debe superar los {1} caracteres")]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string CodPostal { get; set; } = string.Empty;

        [StringLength(100)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? Poblacion { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? Provincia { get; set; }

        [Display(Name = "país")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El {0} es obligatorio")]
        [StringLength(10, ErrorMessage = "El {0} no debe superar los {1} caracteres")]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string Pais { get; set; } = string.Empty;

        [StringLength(25)]
        [RegularExpression(@"^[\d\+\-\s]*$", ErrorMessage = "El teléfono debe tener un formato válido")]
        public string? Telefono { get; set; }

        [StringLength(25)]
        [RegularExpression(@"^[\d\+\-\s]*$", ErrorMessage = "El fax debe tener un formato válido")]
        public string? Fax { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^$|^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "El email no tiene un formato válido")]
        public string? EMail { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^$|^(https?://)?(www\.)?[a-zA-Z0-9-]+\.[a-zA-Z]{2,}(/.*)?$", ErrorMessage = "La web no tiene un formato válido")]
        public string? Web { get; set; }

        [StringLength(25)]
        [RegularExpression(@"^[\d\+\-\s]*$", ErrorMessage = "El teléfono 2 debe tener un formato válido")]
        public string? Telefono2 { get; set; }

        [StringLength(25)]
        [RegularExpression(@"^[\d\+\-\s]*$", ErrorMessage = "El móvil debe tener un formato válido")]
        public string? Movil { get; set; }

        [Display(Name = "forma de pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La {0} es obligatoria")]
        [StringLength(10, ErrorMessage = "La {0} no debe superar los {1} caracteres")]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string FormaPago { get; set; } = string.Empty;

        [StringLength(10)]
        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? CcCliente { get; set; }

        [Display(Name = "descuento comercial")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El {0} es obligatorio")]
        [Range(0, 100, ErrorMessage = "El {0} debe estar entre 0 y 100")]
        public decimal DtoComercial { get; set; }

        [RegularExpression(@"^[\p{L}\p{N}\p{P}\p{Z}]*$", ErrorMessage = "No se permiten caracteres especiales ni emojis")]
        public string? Observaciones { get; set; }
    }
}
