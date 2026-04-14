namespace src.DTOs.Articulos
{
    // Datos de salida de un artículo. Incluye todos los campos de la entidad
    public class ArticuloResponseDto
    {
        public string IdArticulo { get; set; } = string.Empty;
        public string DescArticulo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Familia { get; set; } = string.Empty;
        public string? CCVenta { get; set; }
        public string? CCCompra { get; set; }
        public string? TipoIva { get; set; }
        public string? UdVenta { get; set; }
        public string? UdCompra { get; set; }
        public decimal PesoNeto { get; set; }
        public decimal PesoBruto { get; set; }
        public decimal Plazo { get; set; }
        public decimal Volumen { get; set; }
    }
}
