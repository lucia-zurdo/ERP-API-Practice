using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SegundaAPI.Data;
using SegundaAPI.DTOs.FacturasVenta;
using SegundaAPI.Models;
using SegundaAPI.Services;

namespace SegundaAPI.Controllers
{
    /* Controller para la gestión de facturas de venta
     * Permite crear, leer, actualizar y eliminar facturas */

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    [Authorize]
    public class FacturasVentaController : ControllerBase
    {
        private readonly AppDbContext _context;

        /* El AppDbContext se inyecta automáticamente gracias
         * al registro realizado en Program.cs como Scoped*/
        public FacturasVentaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = "facturas-venta:read")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var facturas = await _context.FacturasVentaCabecera
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
        [Authorize(Policy = "facturas-venta:read")]
        public async Task<IActionResult> GetByNFactura(string nFactura)
        {
            try
            {
                var factura = await _context.FacturasVentaCabecera
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
        [Authorize(Policy = "facturas-venta:write")]
        public async Task<IActionResult> Create([FromBody] FacturaVentaCabeceraCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Validar que el cliente existe
                var cliente = await _context.Clientes.FindAsync(dto.IdCliente);
                if (cliente == null)
                    return BadRequest(new { mensaje = $"No existe ningún cliente con ID '{dto.IdCliente}'" });

                
                // Validar que existe el artículo con ese ID
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

                // Validar que el número de factura no está duplicado
                var nFacturaDuplicado = await _context.FacturasVentaCabecera
                    .AnyAsync(f => f.NFactura == dto.NFactura);
                if (nFacturaDuplicado)
                    return BadRequest(new { mensaje = $"Ya existe una factura de venta con número '{dto.NFactura}'" });

                /*Tipos de IVA de las líneas
                 * Se consulta en la BD el % de IVA de cada artículo referenciado en las líneas*/
                var tiposIva = await ObtenerTiposIva(dto.Lineas
                    .Where(l => l.TipoIva != null)
                    .Select(l => l.TipoIva!)
                    .Distinct()
                    .ToList());

                //Se contruye la cabecera copiando datos del cliente como histórico
                var cabecera = new FacturaVentaCabecera
                {
                    NFactura = dto.NFactura,
                    FechaFactura = dto.FechaFactura,
                    IdCliente = cliente.IdCliente,
                    CifCliente = cliente.CifCliente,
                    RazonSocial = cliente.RazonSocial,
                    FormaPago = !string.IsNullOrEmpty(dto.FormaPago) ? dto.FormaPago : cliente.FormaPago,
                    Estado = dto.Estado,
                    FechaVencimiento = dto.FechaVencimiento,
                    Observaciones = dto.Observaciones
                };

                //Calcular y construir las líneas
                foreach (var lineaDto in dto.Lineas)
                {
                    var importe = FacturaCalculoService.CalcularImpLinea(
                        lineaDto.Cantidad,
                        lineaDto.Precio,
                        lineaDto.Dto,
                        lineaDto.DtoProntoPago);

                    cabecera.Lineas.Add(new FacturaVentaLinea
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

                //Calcular todos los importes de la cabecera
                FacturaCalculoService.CalcularCabecera(
                    cabecera,
                    dto.DtoFactura,
                    dto.DtoProntoPago,
                    dto.RecFinan,
                    dto.RetencionIRPF,
                    tiposIva);

                _context.FacturasVentaCabecera.Add(cabecera);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = $"Factura de venta '{dto.NFactura}' creada correctamente",
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
        [Authorize(Policy = "facturas-venta:write")]
        public async Task<IActionResult> Update(int id, [FromBody] FacturaVentaUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var factura = await _context.FacturasVentaCabecera
                    .Include(f => f.Lineas)
                    .FirstOrDefaultAsync(f => f.IdFactura == dto.IdFactura);

                if (factura == null)
                    return BadRequest(new { mensaje = $"No se encontró ninguna factura de venta con ID '{dto.IdFactura}'" });

                //Validar número de factura duplicado en otra factura distinta
                var nFacturaDuplicado = await _context.FacturasVentaCabecera
                    .AnyAsync(f => f.NFactura == dto.NFactura && f.IdFactura != dto.IdFactura);
                if (nFacturaDuplicado)
                    return BadRequest(new { mensaje = $"Ya existe otra factura de venta con número '{dto.NFactura}'" });

                var tiposIva = await ObtenerTiposIva(dto.Lineas
                    .Where(l => l.TipoIva != null)
                    .Select(l => l.TipoIva!)
                    .Distinct()
                    .ToList());

                //Actualizar cabecera
                factura.NFactura = dto.NFactura;
                factura.FechaFactura = dto.FechaFactura ?? DateTime.Now;
                factura.IdCliente = dto.IdCliente;
                factura.CifCliente = factura.CifCliente;
                factura.RazonSocial = factura.RazonSocial;
                factura.FormaPago = dto.FormaPago;
                factura.Estado = dto.Estado;
                factura.FechaVencimiento = dto.FechaVencimiento;
                factura.Observaciones = dto.Observaciones;

                //Eliminar líneas antiguas y recrearlas
                _context.FacturasVentaLinea.RemoveRange(factura.Lineas);
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

                    factura.Lineas.Add(new FacturaVentaLinea
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
                    mensaje = $"Factura de venta '{dto.NFactura}' actualizada correctamente",
                    impTotal = factura.ImpTotal
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Policy = "facturas-venta:delete")]
        public async Task<IActionResult> Delete([FromBody] int idFactura)
        {
            try
            {
                var factura = await _context.FacturasVentaCabecera
                    .Include(f => f.Lineas)
                    .FirstOrDefaultAsync(f => f.IdFactura == idFactura);

                if (factura == null)
                    return BadRequest(new { mensaje = $"No se encontró ninguna factura de venta con ID '{idFactura}'" });

                _context.FacturasVentaCabecera.Remove(factura);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"Factura de venta con ID '{idFactura}' eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // Mapea la entidad al DTO de respuesta
        private static FacturaVentaCabeceraResponseDto MapToResponseDto(FacturaVentaCabecera f)
        {
            return new FacturaVentaCabeceraResponseDto
            {
                IdFactura = f.IdFactura,
                NFactura = f.NFactura,
                FechaFactura = f.FechaFactura,
                IdCliente = f.IdCliente,
                CifCliente = f.CifCliente,
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
                Lineas = f.Lineas.Select(l => new FacturaVentaLineaResponseDto
                {
                    IdLineaFactura = l.IdLineaFactura,
                    IdFactura = l.IdFactura,
                    IdArticulo = l.IdArticulo,
                    DescArticulo = l.DescArticulo,
                    TipoIva = l.TipoIva,
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

        // Construye el diccionario de tipos de IVA consultando la BD
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
