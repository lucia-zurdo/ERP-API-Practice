using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Models;
using src.Data;
using src.DTOs.Clientes;

namespace src.Controllers
{
    /* Controller para la gestión de clientes
     * Permite crear, leer, actualizar y eliminar clientes
     * El IdCliente es alfanumérico y lo define el usuario, no es autonumérico*/

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        /* El AppDbContext se inyecta automáticamente gracias
         * al registro realizado en Program.cs como Scoped */
        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene todos los clientes de la base de datos
        [HttpGet]
        [Authorize(Policy = "clientes:read")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cliente = await _context.Clientes.ToListAsync();

                /* Se mapea la entidad al DTO de respuesta 
                 * para no exponer directamente el modelo de BD */
                var response = cliente.Select(c => new ClientesResponseDto
                {
                    IdCliente = c.IdCliente,
                    DescCliente = c.DescCliente,
                    RazonSocial = c.RazonSocial,
                    CifCliente = c.CifCliente,
                    Direccion = c.Direccion,
                    CodPostal = c.CodPostal,
                    Poblacion = c.Poblacion,
                    Provincia = c.Provincia,
                    Pais = c.Pais,
                    Telefono = c.Telefono,
                    Fax = c.Fax,
                    EMail = c.EMail,
                    Web = c.Web,
                    Telefono2 = c.Telefono2,
                    Movil = c.Movil,
                    FormaPago = c.FormaPago,
                    CcCliente = c.CcCliente,
                    DtoComercial = c.DtoComercial,
                    Observaciones = c.Observaciones

                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }

        }

        /* Obtiene un cliente concreto por su CIF
         * Devuelve 400 si el cliente no existe*/
        [HttpGet("buscar/cif/{cifCliente}")]
        [Authorize(Policy = "clientes:read")]
        public async Task<IActionResult> GetClienteByCif(string cifCliente)
        {
            try
            {
                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.CifCliente == cifCliente);

                if (cliente == null)
                    return BadRequest(new { mensaje = $"No se encontró ningún cliente con CIF '{cifCliente}'" });

                var response = new ClientesResponseDto
                {
                    IdCliente = cliente.IdCliente,
                    DescCliente = cliente.DescCliente,
                    RazonSocial = cliente.RazonSocial,
                    CifCliente = cliente.CifCliente,
                    Direccion = cliente.Direccion,
                    CodPostal = cliente.CodPostal,
                    Poblacion = cliente.Poblacion,
                    Provincia = cliente.Provincia,
                    Pais = cliente.Pais,
                    Telefono = cliente.Telefono,
                    Fax = cliente.Fax,
                    EMail = cliente.EMail,
                    Web = cliente.Web,
                    Telefono2 = cliente.Telefono2,
                    Movil = cliente.Movil,
                    FormaPago = cliente.FormaPago,
                    CcCliente = cliente.CcCliente,
                    DtoComercial = cliente.DtoComercial,
                    Observaciones = cliente.Observaciones
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /* Obtiene un cliente concreto por su razón social
         * Devuelve 400 si el cliente no existe */
        [HttpGet("buscar/razonsocial/{razonSocial}")]
        [Authorize(Policy = "clientes:read")]
        public async Task<IActionResult> GetClienteByRazonSocial(string razonSocial)
        {
            try
            {
                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.RazonSocial == razonSocial);

                if (cliente == null)
                    return BadRequest(new { mensaje = $"No se encontró ningún cliente con razón social '{razonSocial}'" });

                var response = new ClientesResponseDto
                {
                    IdCliente = cliente.IdCliente,
                    DescCliente = cliente.DescCliente,
                    RazonSocial = cliente.RazonSocial,
                    CifCliente = cliente.CifCliente,
                    Direccion = cliente.Direccion,
                    CodPostal = cliente.CodPostal,
                    Poblacion = cliente.Poblacion,
                    Provincia = cliente.Provincia,
                    Pais = cliente.Pais,
                    Telefono = cliente.Telefono,
                    Fax = cliente.Fax,
                    EMail = cliente.EMail,
                    Web = cliente.Web,
                    Telefono2 = cliente.Telefono2,
                    Movil = cliente.Movil,
                    FormaPago = cliente.FormaPago,
                    CcCliente = cliente.CcCliente,
                    DtoComercial = cliente.DtoComercial,
                    Observaciones = cliente.Observaciones
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /* Crea un nuevo cliente
         * Valida que el IdCliente y el CIF no estén duplicados antes de insertar*/
        [HttpPost]
        [Authorize(Policy = "clientes:write")]
        public async Task<IActionResult> Create([FromBody] ClienteCreateDto dto)
        {
            // Validar Model State para comprobar que todos los datos son correctos
            // ModelState valida automáticamente todas las anotaciones del DTO
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                // Verificar que no existe un cliente con ese CIF
                var existeCif = await _context.Clientes.AnyAsync(c => c.CifCliente == dto.CifCliente);
                if (existeCif)
                    return BadRequest(new { mensaje = $"Ya existe un cliente con ese CIF '{dto.CifCliente}'" });

                // Generación automática del ID
                var ultimo = await _context.Clientes
                    .OrderByDescending(c => c.IdCliente)
                    .FirstOrDefaultAsync();

                string nuevoId;
                 if (ultimo == null)
                 {
                    nuevoId = "CLI001";
                 }
                 else
                 {
                    int numero = int.Parse(ultimo.IdCliente.Substring(3));
                    nuevoId = "CLI" + (numero + 1).ToString("D3");
                 }

                var cliente = new Clientes
                {
                    IdCliente = nuevoId,
                    DescCliente = dto.DescCliente,
                    RazonSocial = dto.RazonSocial,
                    CifCliente = dto.CifCliente,
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
                    CcCliente = dto.CcCliente,
                    DtoComercial = dto.DtoComercial,
                    Observaciones = dto.Observaciones,
                };

                // Se añade el cliente
                _context.Clientes.Add(cliente);

                // Se guardan los cambios
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"Cliente '{nuevoId}' creado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        /* Modifica un cliente existente
         * El IdCliente del body identifica qué cliente se va a modificar
         * Valida que el nuevo CIF no esté en uso por otro cliente distinto*/
        [HttpPut]
        [Authorize(Policy = "clientes:write")]
        public async Task<IActionResult> Update([FromBody] ClienteUpdateDto dto)
        {
            // Validar Model State para comprobar que todos los datos son correctos
            // ModelState valida automáticamente todas las anotaciones del DTO
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // FindAsync busca por clave primaria y devuelve null si no existe. Comprueba que el cliente no es null
                var cliente = await _context.Clientes.FindAsync(dto.IdCliente);
                if (cliente == null)
                    return BadRequest(new { mensaje = $"No se encontró ningún cliente con ID '{dto.IdCliente}'" });

                //Actualizar campos
                cliente.DescCliente = dto.DescCliente;
                cliente.RazonSocial = dto.RazonSocial;
                cliente.CifCliente = dto.CifCliente;
                cliente.Direccion = dto.Direccion;
                cliente.CodPostal = dto.CodPostal;
                cliente.Poblacion = dto.Poblacion;
                cliente.Provincia = dto.Provincia;
                cliente.Pais = dto.Pais;
                cliente.Telefono = dto.Telefono;
                cliente.Fax = dto.Fax;
                cliente.EMail = dto.EMail;
                cliente.Web = dto.Web;
                cliente.Telefono2 = dto.Telefono2;
                cliente.Movil = dto.Movil;
                cliente.FormaPago = dto.FormaPago;
                cliente.CcCliente = dto.CcCliente;
                cliente.DtoComercial = dto.DtoComercial;
                cliente.Observaciones = dto.Observaciones;

                // Se guardan los cambios
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"Cliente '{dto.IdCliente}' actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // Elimina un cliente por su ID
        // Devuelve 400 si el cliente no existe
        [HttpDelete]
        [Authorize(Policy = "clientes:delete")]
        public async Task<IActionResult> Delete([FromBody] string IdCliente)
        {
            try
            {
                // FindAsync busca por clave primaria y devuelve null si no existe. Comprueba que el cliente no es null
                var cliente = await _context.Clientes.FindAsync(IdCliente);
                if (cliente == null)
                    return BadRequest(new { mensaje = $"No se encontró ningún cliente con ID '{IdCliente}'" });

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = $"Cliente '{IdCliente}' eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }
}
