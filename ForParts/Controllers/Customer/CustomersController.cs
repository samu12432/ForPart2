using ForParts.DTOs.Customer;
using ForParts.DTOs.Common;
using ForParts.IRepository.Customer;
using ForParts.Models.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForParts.Controllers.Customer
{
    [ApiController]
    [Route("/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerRepository customerRepository, ILogger<CustomersController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CustomerCreateDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrWhiteSpace(dto.Nombre?.Trim()))
                    return BadRequest(new { message = "El nombre es requerido" });

                if (string.IsNullOrWhiteSpace(dto.Identificador?.Trim()))
                    return BadRequest(new { message = "El identificador es requerido" });

                var entity = MapToEntity(dto);
                entity.Nombre = entity.Nombre.Trim();

                var createdEntity = await _customerRepository.AddAsync(entity);
                var result = MapToDto(createdEntity);

                return CreatedAtAction(nameof(GetCustomer), new { id = createdEntity.CustomerId }, result);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
            {
                _logger.LogWarning(ex, "Duplicate identifier attempt: {Identificador}", dto.Identificador);
                return Conflict(new { message = "Ya existe un cliente con este identificador" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer: {Identificador}", dto.Identificador);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _customerRepository.GetByIdAsync(id);
                if (entity == null)
                    return NotFound(new { message = "Cliente no encontrado" });

                return Ok(MapToDto(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer: {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, [FromBody] CustomerUpdateDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrWhiteSpace(dto.Nombre?.Trim()))
                    return BadRequest(new { message = "El nombre es requerido" });

                if (string.IsNullOrWhiteSpace(dto.Identificador?.Trim()))
                    return BadRequest(new { message = "El identificador es requerido" });

                var existingEntity = await _customerRepository.GetByIdAsync(id);
                if (existingEntity == null)
                    return NotFound(new { message = "Cliente no encontrado" });

                UpdateEntityFromDto(existingEntity, dto);
                existingEntity.Nombre = existingEntity.Nombre.Trim();

                await _customerRepository.UpdateAsync(existingEntity);
                var result = MapToDto(existingEntity);

                return Ok(result);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
            {
                _logger.LogWarning(ex, "Duplicate identifier attempt on update: {Id}, {Identificador}", id, dto.Identificador);
                return Conflict(new { message = "Ya existe un cliente con este identificador" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer: {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _customerRepository.GetByIdAsync(id);
                if (entity == null)
                    return NotFound(new { message = "Cliente no encontrado" });

                await _customerRepository.DeleteAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer: {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CustomerListItemDto>>> GetCustomers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? q = null,
            [FromQuery] string sortBy = "Nombre",
            [FromQuery] string sortDir = "asc",
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 20;

                var desc = sortDir?.ToLower() == "desc";
                var (items, total) = await _customerRepository.ListAsync(page, pageSize, q, sortBy, desc);

                var result = new PagedResult<CustomerListItemDto>
                {
                    Total = total,
                    Page = page,
                    PageSize = pageSize,
                    Items = items.Select(MapToListItemDto).ToList()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing customers");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        private Models.Customers.Customer MapToEntity(CustomerCreateDto dto)
        {
            return new Models.Customers.Customer
            {
                Nombre = dto.Nombre,
                Identificador = dto.Identificador,
                TipoDocumento = string.IsNullOrWhiteSpace(dto.TipoDocumento) ? "RUT" : dto.TipoDocumento,
                Email = dto.Email,
                Telefono = dto.Telefono,
                DireccionFiscal = new Direccion
                {
                    Calle = dto.DireccionFiscal.Calle,
                    Numero = dto.DireccionFiscal.Numero,
                    Ciudad = dto.DireccionFiscal.Ciudad,
                    Departamento = dto.DireccionFiscal.Departamento,
                    CodigoPostal = dto.DireccionFiscal.CodigoPostal,
                    Pais = string.IsNullOrWhiteSpace(dto.DireccionFiscal.Pais) ? "Uruguay" : dto.DireccionFiscal.Pais
                }
            };
        }

        private void UpdateEntityFromDto(Models.Customers.Customer entity, CustomerUpdateDto dto)
        {
            entity.Nombre = dto.Nombre;
            entity.Identificador = dto.Identificador;
            entity.TipoDocumento = string.IsNullOrWhiteSpace(dto.TipoDocumento) ? "RUT" : dto.TipoDocumento;
            entity.Email = dto.Email;
            entity.Telefono = dto.Telefono;
            entity.DireccionFiscal.Calle = dto.DireccionFiscal.Calle;
            entity.DireccionFiscal.Numero = dto.DireccionFiscal.Numero;
            entity.DireccionFiscal.Ciudad = dto.DireccionFiscal.Ciudad;
            entity.DireccionFiscal.Departamento = dto.DireccionFiscal.Departamento;
            entity.DireccionFiscal.CodigoPostal = dto.DireccionFiscal.CodigoPostal;
            entity.DireccionFiscal.Pais = string.IsNullOrWhiteSpace(dto.DireccionFiscal.Pais) ? "Uruguay" : dto.DireccionFiscal.Pais;
        }

        private CustomerDto MapToDto(Models.Customers.Customer entity)
        {
            return new CustomerDto
            {
                CustomerId = entity.CustomerId,
                Nombre = entity.Nombre,
                Identificador = entity.Identificador,
                TipoDocumento = entity.TipoDocumento,
                Email = entity.Email,
                Telefono = entity.Telefono,
                DireccionFiscal = new Direccion
                {
                    Calle = entity.DireccionFiscal.Calle,
                    Numero = entity.DireccionFiscal.Numero,
                    Ciudad = entity.DireccionFiscal.Ciudad,
                    Departamento = entity.DireccionFiscal.Departamento,
                    CodigoPostal = entity.DireccionFiscal.CodigoPostal,
                    Pais = entity.DireccionFiscal.Pais
                }
            };
        }

        private CustomerListItemDto MapToListItemDto(Models.Customers.Customer entity)
        {
            return new CustomerListItemDto
            {
                CustomerId = entity.CustomerId,
                Nombre = entity.Nombre,
                Identificador = entity.Identificador,
                Email = entity.Email,
                Telefono = entity.Telefono
            };
        }
    }
}