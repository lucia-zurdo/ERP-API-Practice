using System.ComponentModel.DataAnnotations;

namespace src.DTOs.Clientes
{
    public class ClienteUpdateDto : ClienteCreateDto
    {
        [Required(ErrorMessage = "El ID del cliente es obligatorio")]
        public string IdCliente { get; set; } = string.Empty;
    }
}
