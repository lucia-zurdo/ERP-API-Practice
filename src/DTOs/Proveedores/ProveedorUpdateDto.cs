using System.ComponentModel.DataAnnotations;

namespace src.DTOs.Proveedores
{
    public class ProveedorUpdateDto : ProveedorCreateDto
    {
        [Required(ErrorMessage = "El ID del proveedor es obligatorio")]
        public string IdProveedor { get; set; } = string.Empty;
    }
}
