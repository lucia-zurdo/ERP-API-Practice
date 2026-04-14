using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Models;
using src.Data;
using src.DTOs.Proveedores;

namespace src.Controllers
{
    /* Controller para la gestión de proveedores
     * Permite crear, leer, actualizar y eliminar proveedores
     * El IdProveedor es alfanumérico y lo define el usuario, no es autonumérico*/

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    [Authorize]
    public class ProveedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        /* El AppDbContext se inyecta automáticamente gracias
         * al registro realizado en Program.cs como Scoped*/
        public ProveedoresController(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene todos los proveedores de la base de datos
        [HttpGet]
        [Authorize(Policy = "proveedores:read")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var proveedor = await _context.Proveedores.ToListAsync();

                /* Se mapea la entidad al DTO de respuesta 
                 * para no exponer directamente el modelo de BD*/
                var response = proveedor.Select(p => new ProveedorResponseDto
                {
                    IdProveedor = p.IdProveedor,
                    DescProveedor = p.DescProveedor,
                    RazonSocial = p.RazonSocial,
                    CifProveedor = p.CifProveedor,
                    Direccion = p.Direccion,
                    CodPostal = p.CodPostal,
                    Poblacion = p.Poblacion,
                    Provincia = p.Provincia,
                    Pais = p.Pais,
                    Telefono = p.Telefono,
                    Fax = p.Fax,
                    EMail = p.EMail,
                    Web = p.Web,
                    Telefono2 = p.Telefono2,
                    Movil = p.Movil,
                    FormaPago = p.FormaPago,
                    CcProveedor = p.CcProveedor,
                    DtoComercial = p.DtoComercial,
                    Observaciones = p.Observaciones

                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }

        }

        /* Obtiene un proveedor concreto por su CIF
         * Devuelve 400 si el proveedor no existe */
        [HttpGet("buscar/cif/{cifProveedor}")]
        [Authorize(Policy = "proveedores:read")]
        public async Task<IActionResult> GetProveedorByCif(string cifProveedor)
        {
            try
            {
                var proveedor = await _context.Proveedores
                    .FirstOrDefaultAsync(p => p.CifProveedor == cifProveedor);

                if (proveedor == null)
                    return BadRequest(new { mensaje = $"No se encontró ningún proveedor con CIF '{cifProveedor}'" });

                var response = new ProveedorResponseDto
                {
                    IdProveedor = proveedor.IdProveedor,
                    DescProveedor = proveedor.DescProveedor,
                    RazonSocial = proveedor.RazonSocial,
                    CifProveedor = proveedor.CifProveedor,
                    Direccion = proveedor.Direccion,
                    CodPostal = proveedor.CodPostal,
                    Poblacion = proveedor.Poblacion,
                    Provincia = proveedor.Provincia,
                    Pais = proveedor.Pais,
                    Telefono = proveedor.Telefono,
                    Fax = proveedor.Fax,
                    EMail = proveedor.EMail,
                    Web = proveedor.Web,
                    Telefono2 = proveedor.Telefono2,
                    Movil = proveedor.Movil,
                    FormaPago = proveedor.FormaPago,
                    CcProveedor = proveedor.CcProveedor,
                    DtoComercial = proveedor.DtoComercial,
                    Observaciones = proveedor.Observaciones
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /* Obtiene un proveedor concreto por su razón social
         * Devuelve 400 si el cliente no existe */
        [HttpGet("buscar/razonsocial/{razonSocial}")]
        [Authorize(Policy = "proveedores:read")]
        public async Task<IActionResult> GetProveedorByRazonSocial(string razonSocial)
        {
            try
            {
                var proveedor = await _context.Proveedores
                    .FirstOrDefaultAsync(p => p.RazonSocial == razonSocial);

                if (proveedor == null)
                    return BadRequest(new { mensaje = $"No se encontró ningún proveedor con razón social '{razonSocial}'" });

                var response = new ProveedorResponseDto
                {
                    IdProveedor = proveedor.IdProveedor,
                    DescProveedor = proveedor.DescProveedor,
                    RazonSocial = proveedor.RazonSocial,
                    CifProveedor = proveedor.CifProveedor,
                    Direccion = proveedor.Direccion,
                    CodPostal = proveedor.CodPostal,
                    Poblacion = proveedor.Poblacion,
                    Provincia = proveedor.Provincia,
                    Pais = proveedor.Pais,
                    Telefono = proveedor.Telefono,
                    Fax = proveedor.Fax,
                    EMail = proveedor.EMail,
                    Web = proveedor.Web,
                    Telefono2 = proveedor.Telefono2,
                    Movil = proveedor.Movil,
                    FormaPago = proveedor.FormaPago,
                    CcProveedor = proveedor.CcProveedor,
                    DtoComercial = proveedor.DtoComercial,
                    Observaciones = proveedor.Observaciones
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /* Crea un nuevo proveedor
         * Valida que el IdProveedor y el CIF no estén duplicados antes de insertar*/
        [HttpPost]
        [Authorize(Policy = "proveedores:write")]
        public async Task<IActionResult> Create([FromBody] ProveedorCreateDto dto)
        {
            // Validar Model State para comprobar que todos los datos son correctos
            // ModelState valida automáticamente todas las anotaciones del DTO
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                // Verificar que no existe un proveedor con ese CIF
                var existeCif = await _context.Proveedores.AnyAsync(p => p.CifProveedor == dto.CifProveedor);
                if (existeCif)
                    return BadRequest(new { mensaje = $"Ya existe un proveedor con ese CIF '{dto.CifProveedor}'" });

                // Generación automática del ID
                var ultimo = await _context.Proveedores
                    .OrderByDescending(p => p.IdProveedor)
                    .FirstOrDefaultAsync();

                string nuevoId;
                if (ultimo == null)
                {
                    nuevoId = "PRO001";
                }
                else
                {
                    int numero = int.Parse(ultimo.IdProveedor.Substring(3));
                    nuevoId = "PRO" + (numero + 1).ToString("D3");
                }

                var proveedor = new Proveedores
                {
                    IdProveedor = nuevoId,
                    DescProveedor = dto.DescProveedor,
                    RazonSocial = dto.RazonSocial,
                    CifProveedor = dto.CifProveedor,
                    Direccion = dto.Direccion,
                    CodPostal = dto.CodPostal,
                    Poblacion = dto.Poblacion,
                    Provincia = dto.Provincia,
                    Pais = dto.Pais,
                    Telefono = dto.Telefono,
                    Fax = dto.Fax,
                    EMail = dto.EMail,
                    Web = dto.Web,
                    Telefono2 = dto.Telefono2,
                    Movil = dto.Movil,
                    FormaPago = dto.FormaPago,
                    CcProveedor = dto.CcProveedor,
                    DtoComercial = dto.DtoComercial,
                    Observaciones = dto.Observaciones,
                };

                // Se añade el proveedor
                _context.Proveedores.Add(proveedor);

                // Se guardan los cambios
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"Proveedor '{nuevoId}' creado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /* Modifica un proveedor existente
         * El IdProveedor del body identifica qué proveedor se va a modificar
         * Valida que el nuevo CIF no esté en uso por otro proveedor distinto*/
        [HttpPut]
        [Authorize(Policy = "proveedores:write")]
        public async Task<IActionResult> Update([FromBody] ProveedorUpdateDto dto)
        {
            // Validar Model State para comprobar que todos los datos son correctos
            // ModelState valida automáticamente todas las anotaciones del DTO
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // FindAsync busca por clave primaria y devuelve null si no existe. Comprueba que el proveedor no es null
                var proveedor = await _context.Proveedores.FindAsync(dto.IdProveedor);
                if (proveedor == null)
                    return BadRequest(new { mensaje = $"No se encontró ningún proveedor con ID '{dto.IdProveedor}'" });

                //Actualizar campos
                proveedor.DescProveedor = dto.DescProveedor;
                proveedor.RazonSocial = dto.RazonSocial;
                proveedor.CifProveedor = dto.CifProveedor;
                proveedor.Direccion = dto.Direccion;
                proveedor.CodPostal = dto.CodPostal;
                proveedor.Poblacion = dto.Poblacion;
                proveedor.Provincia = dto.Provincia;
                proveedor.Pais = dto.Pais;
                proveedor.Telefono = dto.Telefono;
                proveedor.Fax = dto.Fax;
                proveedor.EMail = dto.EMail;
                proveedor.Web = dto.Web;
                proveedor.Telefono2 = dto.Telefono2;
                proveedor.Movil = dto.Movil;
                proveedor.FormaPago = dto.FormaPago;
                proveedor.CcProveedor = dto.CcProveedor;
                proveedor.DtoComercial = dto.DtoComercial;
                proveedor.Observaciones = dto.Observaciones;

                // Se guardan los cambios
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"Proveedor '{dto.IdProveedor}' actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // Elimina un proveedor por su ID
        // Devuelve 400 si el proveedor no existe
        [HttpDelete]
        [Authorize(Policy = "proveedores:delete")]
        public async Task<IActionResult> Delete([FromBody] string IdProveedor)
        {
            try
            {
                // FindAsync busca por clave primaria y devuelve null si no existe. Comprueba que el proveedor no es null
                var proveedor = await _context.Proveedores.FindAsync(IdProveedor);
                if (proveedor == null)
                    return BadRequest(new { mensaje = $"No se encontró ningún proveedor con ID '{IdProveedor}'" });

                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"Proveedor '{IdProveedor}' eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }
}
