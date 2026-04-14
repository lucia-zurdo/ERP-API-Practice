namespace src.DTOs.Proveedores
{
    // Datos de salida de un proveedor. Incluye todos los campos de la entidad
    public class ProveedorResponseDto
    {
        public string IdProveedor { get; set; } = string.Empty;
        public string? DescProveedor { get; set; }
        public string? RazonSocial { get; set; }
        public string CifProveedor { get; set; } = string.Empty;
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
        public string? CcProveedor { get; set; }
        public decimal DtoComercial { get; set; }
        public string? Observaciones { get; set; }
    }
}
