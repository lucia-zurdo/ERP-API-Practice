using System.ComponentModel.DataAnnotations;

namespace src.DTOs.FacturasCompra
{
    /* Datos de entrada para modificar una factura de compra existente
     * Hereda todos los campos y validaciones de FacturaCompraCabeceraCreateDto*/
    public class FacturaCompraUpdateDto : FacturaCompraCabeceraCreateDto
    {
        [Required(ErrorMessage = "El ID de factura es obligatorio para modificar")]
        public int IdFactura { get; set; }
    }
}
