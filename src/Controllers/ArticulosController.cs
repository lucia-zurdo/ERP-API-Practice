using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Models;
using src.Data;
using src.DTOs.Articulos;

namespace src.Controllers
{
    /* Controller para la gestión de artículos
     * Permite crear, leer, actualizar y eliminar artículos
     * El IdArticulo es alfanumérico y lo define el usuario, no es autonumérico*/

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    [Authorize]
    public class ArticulosController : ControllerBase
    {
        private readonly AppDbContext _context;

        /* El AppDbContext se inyecta automáticamente gracias
         * al registro realizado en Program.cs como Scoped*/
        public ArticulosController(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene todos los artículos de la base de datos
        [HttpGet]
        [Authorize(Policy = "articulos:read")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var articulo = await _context.Articulos.ToListAsync();

                /* Se mapea la entidad al DTO de respuesta 
                 * para no exponer directamente el modelo de BD*/
                var response = articulo.Select(a => new ArticuloResponseDto
                {
                    IdArticulo = a.IdArticulo,
                    DescArticulo = a.DescArticulo,
                    Estado = a.Estado,
                    Familia = a.Familia,
                    CCVenta = a.CCVenta,
                    CCCompra = a.CCCompra,
                    TipoIva = a.TipoIva,
                    UdVenta = a.UdVenta,
                    UdCompra = a.UdCompra,
                    PesoNeto = a.PesoNeto,
                    PesoBruto = a.PesoBruto,
                    Plazo = a.Plazo,
                    Volumen = a.Volumen,
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }

        }

        /* Obtiene un artículo concreto por su ID
         * Devuelve 400 si el artículo no existe*/
        [HttpGet("buscar/descripcion/{DescArticulo}")]
        [Authorize(Policy = "articulos:read")]
        public async Task<IActionResult> GetArticuloByDesc(string descArticulo)
        {
            try
            {
                var articulo = await _context.Articulos
                    .FirstOrDefaultAsync(a => a.DescArticulo == descArticulo);

                if (articulo == null)
                    return BadRequest(new { mensaje = $"No se encontró ningún artículo con descripción '{descArticulo}'" });

                var response = new ArticuloResponseDto
                {
                    IdArticulo = articulo.IdArticulo,
                    DescArticulo = articulo.DescArticulo,
                    Estado = articulo.Estado,
                    Familia = articulo.Familia,
                    CCVenta = articulo.CCVenta,
                    CCCompra = articulo.CCCompra,
                    TipoIva = articulo.TipoIva,
                    UdVenta = articulo.UdVenta,
                    UdCompra = articulo.UdCompra,
                    PesoNeto = articulo.PesoNeto,
                    PesoBruto = articulo.PesoBruto,
                    Plazo = articulo.Plazo,
                    Volumen = articulo.Volumen,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /* Crea un nuevo artículo
         * Valida que el IdArticulo no esté duplicado antes de insertar*/
        [HttpPost]
        [Authorize(Policy = "articulos:write")]
        public async Task<IActionResult> Create([FromBody] ArticuloResponseDto dto)
        {
            // Validar Model State para comprobar que todos los datos son correctos
            // ModelState valida automáticamente todas las anotaciones del DTO
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                // Generación automática del ID
                var ultimo = await _context.Articulos
                    .OrderByDescending(a => a.IdArticulo)
                    .FirstOrDefaultAsync();

                string nuevoId;
                if (ultimo == null)
                {
                    nuevoId = "ART001";
                }
                else
                {
                    int numero = int.Parse(ultimo.IdArticulo.Substring(3));
                    nuevoId = "ART" + (numero + 1).ToString("D3");
                }

                var articulo = new Articulos
                {
                    IdArticulo = nuevoId,
                    DescArticulo = dto.DescArticulo,
                    Estado = dto.Estado,
                    Familia = dto.Familia,
                    CCVenta = dto.CCVenta,
                    CCCompra = dto.CCCompra,
                    TipoIva = dto.TipoIva,
                    UdVenta = dto.UdVenta,
                    UdCompra = dto.UdCompra,
                    PesoNeto = dto.PesoNeto,
                    PesoBruto = dto.PesoBruto,
                    Plazo = dto.Plazo,
                    Volumen = dto.Volumen,
                };

                // Se añade el artículo
                _context.Articulos.Add(articulo);

                // Se guardan los cambios
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"Artículo '{nuevoId}' creado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /* Modifica un proveedor existente
         * El IdArticulo del body identifica qué artículo se va a modificar*/
        [HttpPut]
        [Authorize(Policy = "articulos:write")]
        public async Task<IActionResult> Update([FromBody] ArticuloUpdateDto dto)
        {
            // Validar Model State para comprobar que todos los datos son correctos
            // ModelState valida automáticamente todas las anotaciones del DTO
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // FindAsync busca por clave primaria y devuelve null si no existe. Comprueba que el artículo no es null
                var articulo = await _context.Articulos.FindAsync(dto.IdArticulo);
                if (articulo == null)
                    return BadRequest(new { mensaje = $"No se encontró ningún artículo con ID '{dto.IdArticulo}'" });

                //Actualizar campos
                articulo.DescArticulo = dto.DescArticulo;
                articulo.Estado = dto.Estado;
                articulo.Familia = dto.Familia;
                articulo.CCVenta = dto.CCVenta;
                articulo.CCCompra = dto.CCCompra;
                articulo.TipoIva = dto.TipoIva;
                articulo.UdVenta = dto.UdVenta;
                articulo.UdCompra = dto.UdCompra;
                articulo.PesoNeto = dto.PesoNeto;
                articulo.PesoBruto = dto.PesoBruto;
                articulo.Plazo = dto.Plazo;
                articulo.Volumen = dto.Volumen;

                // Se guardan los cambios
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"Artículo '{dto.IdArticulo}' actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
            }
        }

        // Elimina un artículo por su ID
        // Devuelve 400 si el artículo no existe
        [HttpDelete("eliminar/descripcion/{DescArticulo}")]
        [Authorize(Policy = "articulos:delete")]
        public async Task<IActionResult> Delete([FromBody] string descArticulo)
        {
            try
            {
                // FindAsync busca por clave primaria y devuelve null si no existe. Comprueba que el proveedor no es null
                var articulo = await _context.Articulos.FindAsync(descArticulo);
                if (articulo == null)
                    return BadRequest(new { mensaje = $"No se encontró ningún artículo '{descArticulo}'" });

                _context.Articulos.Remove(articulo);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"'{descArticulo}' eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }
}
