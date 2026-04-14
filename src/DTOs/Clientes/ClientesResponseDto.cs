namespace src.DTOs.Clientes
{
    // Datos de salida de un cliente. Incluye todos los campos de la entidad
    public class ClientesResponseDto
    {
        public string IdCliente { get; set; } = string.Empty;
        public string DescCliente { get; set; } = string.Empty;
        public string? RazonSocial { get; set; }
        public string CifCliente { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string CodPostal { get; set; } = string.Empty;
        public string? Poblacion { get; set; }
        public string? Provincia { get; set; }
        public string Pais { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Fax { get; set; }
        public string? EMail { get; set; }
        public string? Web { get; set; }
        public string? Telefono2 { get; set; }
        public string? Movil { get; set; }
        public string FormaPago { get; set; } = string.Empty;
        public string? CcCliente { get; set; }
        public decimal DtoComercial { get; set; }
        public string? Observaciones { get; set; }
    }
}
