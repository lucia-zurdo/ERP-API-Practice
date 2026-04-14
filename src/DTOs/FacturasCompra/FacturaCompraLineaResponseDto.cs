namespace src.DTOs.FacturasCompra
{
    /* Datos de salida que devuelve la API tras una operación exitosa
     * Incluye todos los campos de la entidad, incluidos los calculados automáticamente 
     * por FacturaCalculoService que no están en el CreateDto*/
    public class FacturaCompraLineaResponseDto
    {
        public int IdLineaFactura { get; set; }
        public int IdFactura { get; set; }
        public string? IdArticulo { get; set; }
        public string? DescArticulo { get; set; }
        public string? TipoIVA { get; set; }
        public string IdUdMedida { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Dto { get; set; }
        public decimal DtoProntoPago { get; set; }
        public decimal Importe { get; set; }
        public string? Lote { get; set; }
        public string? Observaciones { get; set; }
    }
}
