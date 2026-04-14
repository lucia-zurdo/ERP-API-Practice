namespace src.DTOs.FacturasCompra
{
    /* Datos de salida que devuelve la API tras una operación exitosa
     * Incluye todos los campos de la entidad, incluidos los calculados automáticamente 
     * por FacturaCalculoService que no están en el CreateDto*/
    public class FacturaCompraCabeceraResponseDto
    {
        public int IdFactura { get; set; }
        public string NFactura { get; set; } = string.Empty;
        public DateTime? FechaFactura { get; set; }
        public string IdProveedor { get; set; } = string.Empty;
        public string? CifProveedor { get; set; }
        public string? RazonSocial { get; set; }
        public string? FormaPago { get; set; }
        public int Estado { get; set; }
        public decimal ImpLineas { get; set; }
        public decimal DtoFactura { get; set; }
        public decimal ImpDtoFactura { get; set; }
        public decimal DtoProntoPago { get; set; }
        public decimal ImpDpp { get; set; }
        public decimal RecFinan { get; set; }
        public decimal ImpRecFinan { get; set; }
        public decimal ImpRE { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal ImpIva { get; set; }
        public decimal RetencionIRPF { get; set; }
        public decimal ImpRetencion { get; set; }
        public decimal ImpTotal { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string? Observaciones { get; set; }
        public List<FacturaCompraLineaResponseDto> Lineas { get; set; } = new();
    }
}
