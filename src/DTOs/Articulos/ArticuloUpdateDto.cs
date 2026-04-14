using System.ComponentModel.DataAnnotations;

namespace src.DTOs.Articulos
{
    public class ArticuloUpdateDto : ArticuloCreateDto
    {
        [Required(ErrorMessage = "El ID del artículo es obligatorio")]
        public string IdArticulo { get; set; } = string.Empty;
    }
}
