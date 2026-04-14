using System.ComponentModel.DataAnnotations;

namespace src.DTOs.FacturasVenta
{
    /* Datos de entrada para modificar una factura de venta existente
     * Hereda todos los campos y validaciones de FacturaVentaCabeceraCreateDto
     * La única diferencia es IdFactura, necesario para identificar
     * qué factura se va a modificar*/
    public class FacturaVentaUpdateDto : FacturaVentaCabeceraCreateDto
    {
        [Required(ErrorMessage = "El ID de factura es obligatorio para modificar")]
        public int IdFactura { get; set; }
    }
}
