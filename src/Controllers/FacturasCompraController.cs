using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SegundaAPI.Data;
using SegundaAPI.DTOs.FacturasCompra;
using SegundaAPI.Models;
using SegundaAPI.Services;

namespace SegundaAPI.Controllers
{
    /* Controller para la gestión de facturas de compra
     * Permite crear, leer, actualizar y eliminar facturas */

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    [Authorize]
    public class FacturasCompraController : ControllerBase
    {
        private readonly AppDbContext _context;

        /* El AppDbContext se inyecta automáticamente gracias
         * al registro realizado en Program.cs como Scoped*/
        public FacturasCompraController(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene todas las facturas de la base de datos
        [HttpGet]
        [Authorize(Policy = "facturas-compra:read")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var facturas = await _context.FacturasCompraCabecera
                    .Include(f => f.Lineas)
                    .ToListAsync();

                var response = facturas.Select(f => MapToResponseDto(f)).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpGet("buscar/nfactura/{nFactura}")]
        [Authorize(Policy = "facturas-compra:read")]
        public async Task<IActionResult> GetByNFactura(string nFactura)
        {
            try
            {
                var factura = await _context.FacturasCompraCabecera
                    .Include(f => f.Lineas)
                    .FirstOrDefaultAsync(f => f.NFactura == nFactura);

                if (factura == null)
                    return BadRequest(new { mensaje = $"No se encontró ninguna factura con número '{nFactura}'" });

                return Ok(MapToResponseDto(factura));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "facturas-compra:write")]
        public async Task<IActionResult> Create([FromBody] FacturaCompraCabeceraCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var proveedor = await _context.Proveedores.FindAsync(dto.IdProveedor);
                if (proveedor == null)
                    return BadRequest(new { mensaje = $"No existe ningún proveedor con ID '{dto.IdProveedor}'" });

                var idsArticulos = dto.Lineas
                    .Where(l => !string.IsNullOrEmpty(l.IdArticulo))
                    .Select(l => l.IdArticulo)
                    .Distinct()
                    .ToList();

                foreach (var idArticulo in idsArticulos)
                {
                    var articuloExiste = await _context.Articulos
                        .AnyAsync(a => a.IdArticulo == idArticulo);

                    if (!articuloExiste)
                        return BadRequest(new { mensaje = $"No existe ningún artículo con ID '{idArticulo}'" });
                }

                var nFacturaDuplicado = await _context.FacturasCompraCabecera
                    .AnyAsync(f => f.NFactura == dto.NFactura);
                if (nFacturaDuplicado)
                    return BadRequest(new { mensaje = $"Ya existe una factura de compra con número '{dto.NFactura}'" });

                var tiposIva = await ObtenerTiposIva(dto.Lineas
                    .Where(l => l.TipoIva != null)
                    .Select(l => l.TipoIva!)
                    .Distinct()
                    .ToList());

                var cabecera = new FacturaCompraCabecera
                {
                    NFactura = dto.NFactura,
                    FechaFactura = dto.FechaFactura ?? DateTime.Now,
                    IdProveedor = dto.IdProveedor,
                    CifProveedor = proveedor.CifProveedor,
                    RazonSocial = proveedor.RazonSocial,
                    FormaPago = !string.IsNullOrEmpty(dto.FormaPago) ? dto.FormaPago : proveedor.FormaPago,
                    Estado = dto.Estado,
                    FechaVencimiento = dto.FechaVencimiento,
                    Observaciones = dto.Observaciones
                };

                foreach (var lineaDto in dto.Lineas)
                {
                    var importe = FacturaCalculoService.CalcularImpLinea(
                        lineaDto.Cantidad,
                        lineaDto.Precio,
                        lineaDto.Dto,
                        lineaDto.DtoProntoPago);

                    cabecera.Lineas.Add(new FacturaCompraLinea
                    {
                        IdArticulo = lineaDto.IdArticulo,
                        DescArticulo = lineaDto.DescArticulo,
                        TipoIva = lineaDto.TipoIva,
                        IdUdMedida = lineaDto.IdUdMedida,
                        Cantidad = lineaDto.Cantidad,
                        Precio = lineaDto.Precio,
                        Dto = lineaDto.Dto,
                        DtoProntoPago = lineaDto.DtoProntoPago,
                        Importe = importe,
                        Lote = lineaDto.Lote,
                        Observaciones = lineaDto.Observaciones
                    });
                }

                FacturaCalculoService.CalcularCabecera(
                    cabecera,
                    dto.DtoFactura,
                    dto.DtoProntoPago,
                    dto.RecFinan,
                    dto.RetencionIRPF,
                    tiposIva);

                _context.FacturasCompraCabecera.Add(cabecera);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = $"Factura de compra '{dto.NFactura}' creada correctamente",
                    idFactura = cabecera.IdFactura,
                    impTotal = cabecera.ImpTotal
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "facturas-compra:write")]
        public async Task<IActionResult> Update(int id, [FromBody] FacturaCompraUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var factura = await _context.FacturasCompraCabecera
                    .Include(f => f.Lineas)
                    .FirstOrDefaultAsync(f => f.IdFactura == dto.IdFactura);

                if (factura == null)
                    return BadRequest(new { mensaje = $"No se encontró ninguna factura de compra con ID '{dto.IdFactura}'" });

                var nFacturaDuplicado = await _context.FacturasCompraCabecera
                    .AnyAsync(f => f.NFactura == dto.NFactura && f.IdFactura != dto.IdFactura);
                if (nFacturaDuplicado)
                    return BadRequest(new { mensaje = $"Ya existe otra factura de compra con número '{dto.NFactura}'" });

                var tiposIva = await ObtenerTiposIva(dto.Lineas
                    .Where(l => l.TipoIva != null)
                    .Select(l => l.TipoIva!)
                    .Distinct()
                    .ToList());

                factura.NFactura = dto.NFactura;
                factura.FechaFactura = dto.FechaFactura ?? DateTime.Now;
                factura.IdProveedor = dto.IdProveedor;
                factura.CifProveedor = factura.CifProveedor;
                factura.RazonSocial = factura.RazonSocial;
                factura.FormaPago = dto.FormaPago;
                factura.Estado = dto.Estado;
                factura.FechaVencimiento = dto.FechaVencimiento;
                factura.Observaciones = dto.Observaciones;

                _context.FacturasCompraLinea.RemoveRange(factura.Lineas);
                factura.Lineas.Clear();

                foreach (var lineaDto in dto.Lineas)
                {
                    // Obtiene las líneas originales para comparar
                    var lineasOriginales = factura.Lineas.ToList();

                    
                    // Para validar si el artículo ha cambiado o es una línea nueva
                    var lineaOriginal = lineasOriginales.FirstOrDefault(l => l.IdLineaFactura == lineaDto.IdLineaFactura);
                    if (lineaOriginal == null || lineaOriginal.IdArticulo != lineaDto.IdArticulo)
                    {
                        var articulo = await _context.Articulos.FindAsync(lineaDto.IdArticulo);
                        if (articulo == null)
                            return BadRequest(new { mensaje = $"No existe ningún artículo con ID '{lineaDto.IdArticulo}'" });
                    }

                    var importe = FacturaCalculoService.CalcularImpLinea(
                        lineaDto.Cantidad,
                        lineaDto.Precio,
                        lineaDto.Dto,
                        lineaDto.DtoProntoPago);

                    factura.Lineas.Add(new FacturaCompraLinea
                    {
                        IdFactura = factura.IdFactura,
                        IdArticulo = lineaDto.IdArticulo,
                        DescArticulo = lineaDto.DescArticulo,
                        TipoIva = lineaDto.TipoIva,
                        IdUdMedida = lineaDto.IdUdMedida,
                        Cantidad = lineaDto.Cantidad,
                        Precio = lineaDto.Precio,
                        Dto = lineaDto.Dto,
                        DtoProntoPago = lineaDto.DtoProntoPago,
                        Importe = importe,
                        Lote = lineaDto.Lote,
                        Observaciones = lineaDto.Observaciones
                    });
                }

                FacturaCalculoService.CalcularCabecera(
                        factura,
                        dto.DtoFactura,
                        dto.DtoProntoPago,
                        dto.RecFinan,
                        dto.RetencionIRPF,
                        tiposIva);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = $"Factura de compra '{dto.NFactura}' actualizada correctamente",
                    impTotal = factura.ImpTotal
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Policy = "facturas-compra:delete")]
        public async Task<IActionResult> Delete([FromBody] int idFactura)
        {
            try
            {
                var factura = await _context.FacturasCompraCabecera
                    .Include(f => f.Lineas)
                    .FirstOrDefaultAsync(f => f.IdFactura == idFactura);

                if (factura == null)
                    return BadRequest(new { mensaje = $"No se encontró ninguna factura de compra con ID '{idFactura}'" });

                _context.FacturasCompraCabecera.Remove(factura);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"Factura de compra con ID '{idFactura}' eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        private static FacturaCompraCabeceraResponseDto MapToResponseDto(FacturaCompraCabecera f)
        {
            return new FacturaCompraCabeceraResponseDto
            {
                IdFactura = f.IdFactura,
                NFactura = f.NFactura,
                FechaFactura = f.FechaFactura,
                IdProveedor = f.IdProveedor,
                CifProveedor = f.CifProveedor,
                RazonSocial = f.RazonSocial,
                FormaPago = f.FormaPago,
                Estado = f.Estado,
                ImpLineas = f.ImpLineas,
                DtoFactura = f.DtoFactura,
                ImpDtoFactura = f.ImpDtoFactura,
                DtoProntoPago = f.DtoProntoPago,
                ImpDpp = f.ImpDpp,
                RecFinan = f.RecFinan,
                ImpRecFinan = f.ImpRecFinan,
                ImpRE = f.ImpRE,
                BaseImponible = f.BaseImponible,
                ImpIva = f.ImpIva,
                RetencionIRPF = f.RetencionIRPF,
                ImpRetencion = f.ImpRetencion,
                ImpTotal = f.ImpTotal,
                FechaVencimiento = f.FechaVencimiento,
                Observaciones = f.Observaciones,
                Lineas = f.Lineas.Select(l => new FacturaCompraLineaResponseDto
                {
                    IdLineaFactura = l.IdLineaFactura,
                    IdFactura = l.IdFactura,
                    IdArticulo = l.IdArticulo,
                    DescArticulo = l.DescArticulo,
                    TipoIVA = l.TipoIva,
                    IdUdMedida = l.IdUdMedida,
                    Cantidad = l.Cantidad,
                    Precio = l.Precio,
                    Dto = l.Dto,
                    DtoProntoPago = l.DtoProntoPago,
                    Importe = l.Importe,
                    Lote = l.Lote,
                    Observaciones = l.Observaciones
                }).ToList()
            };
        }

        private async Task<Dictionary<string, decimal>> ObtenerTiposIva(List<string> codigos)
        {
            var mapa = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            foreach (var codigo in codigos)
            {
                mapa[codigo] = codigo.ToUpper() switch
                {
                    "IVA21" => 21m,
                    "IVA10" => 10m,
                    "IVA4" => 4m,
                    "IVA0" => 0m,
                    _ => 0m
                };
            }
            return mapa;
        }
    }
}
