using ForParts.DTOs.Supply;
using ForParts.Exceptions.Supply;
using ForParts.IServices.Image;
using ForParts.IServices.Supply;
using ForParts.Models.Enums;
using ForParts.Services.Supply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForParts.Controllers.Supply
{
    public class SupplyController : Controller
    {
        private readonly ISupplyService<ProfileDto> _profileService;
        private readonly ISupplyService<GlassDto> _glassService;
        private readonly ISupplyService<AccessoryDto> _accessoryService;

        public SupplyController(ISupplyService<ProfileDto> profileService, ISupplyService<GlassDto> glassService, ISupplyService<AccessoryDto> accessoryService)
        {
            _profileService = profileService;
            _glassService = glassService;
            _accessoryService = accessoryService;
        }

        //____________ALTA____________//
        [HttpPost("altaPerfil")]
        [Authorize]
        public async Task<IActionResult> AddProfile([FromForm] ProfileDto dto)
        {
            return await HandleAdd<ProfileDto, ProfileException>(dto,
                _profileService.AddSupplyAsync,
                "Perfil creado correctamente."
            );
        }

        [HttpPost("altaVidrio")]
        [Authorize]
        public async Task<IActionResult> AddGlass([FromForm] GlassDto dto)
        {
            return await HandleAdd<GlassDto, GlassException>(
                dto,
                _glassService.AddSupplyAsync,
                "Vidrio creado correctamente."
            );
        }

        [HttpPost("altaAccesorio")]
        [Authorize]
        public async Task<IActionResult> AddAccessory([FromForm] AccessoryDto dto)
        {
            return await HandleAdd<AccessoryDto, AccessoryException>(
                dto,
                _accessoryService.AddSupplyAsync,
                "Accesorio creado correctamente."
            );
        }


        //ELIMINAR
        [HttpDelete("bajaInsumo")]
        [Authorize]
        public async Task<IActionResult> DeleteSupply([FromBody] DeleteSupplyDto dto)
        {
            try
            {
                bool deleted = dto.type switch
                {
                    TypeSupply.Profile => await _profileService.DeleteByCodeAsync(dto.codeSupply),
                    TypeSupply.Glass => await _glassService.DeleteByCodeAsync(dto.codeSupply),
                    TypeSupply.Accessory => await _accessoryService.DeleteByCodeAsync(dto.codeSupply),
                    _ => throw new InvalidOperationException("Tipo de insumo no válido.")
                };

                return deleted
                    ? Ok(new { message = "Eliminado correctamente." })
                    : BadRequest(new { status = 400, message = "No se pudo eliminar el insumo." });
            }
            catch (SupplyException e)
            {
                return BadRequest(new { status = 400, message = e.Message });
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { status = 400, message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
        }

        //____________EDITAR____________//
        [HttpPut("editarDescripcionInsumo")]
        [Authorize]
        public async Task<IActionResult> UpdateDescription([FromBody] EditSupplyDto dto)
        {
            try
            {
                bool update = dto.type switch
                {
                    TypeSupply.Profile => await _profileService.UpdateDescriptionAsync(dto),
                    TypeSupply.Glass => await _glassService.UpdateDescriptionAsync(dto),
                    TypeSupply.Accessory => await _accessoryService.UpdateDescriptionAsync(dto),
                    _ => throw new InvalidOperationException("Tipo de insumo no válido.")
                };

                return update
                    ? Ok(new { message = "Insumo actualizado correctamente." })
                    : BadRequest(new { status = 400, message = "No se pudo actualizar sel insumo." });
            }
            catch (SupplyException e)
            {
                return BadRequest(new { status = 400, message = e.Message });
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { status = 400, message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
        }

        [HttpPut("editarImagenInsumo")]
        [Authorize]
        public async Task<IActionResult> UpdateImage([FromForm] EditImageSupplyDto dto)
        {
            try
            {
                bool update = dto.type switch
                {
                    TypeSupply.Profile => await _profileService.UpdateImageAsync(dto),
                    TypeSupply.Glass => await _glassService.UpdateImageAsync(dto),
                    TypeSupply.Accessory => await _accessoryService.UpdateImageAsync(dto),
                    _ => throw new InvalidOperationException("Tipo de insumo no válido.")
                };

                return update
                    ? Ok(new { message = "Insumo actualizado correctamente." })
                    : BadRequest(new { status = 400, message = "No se pudo actualizar sel insumo." });
            }
            catch (SupplyException e)
            {
                return BadRequest(new { status = 400, message = e.Message });
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { status = 400, message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
        }

        [HttpPut("editarPrecioInsumo")]
        [Authorize]
        public async Task<IActionResult> UpdatePrice([FromBody] EditPriceSupplyDto dto)
        {
            try
            {
                bool update = dto.type switch
                {
                    TypeSupply.Profile => await _profileService.UpdatePriceAsync(dto),
                    TypeSupply.Glass => await _glassService.UpdatePriceAsync(dto),
                    TypeSupply.Accessory => await _accessoryService.UpdatePriceAsync(dto),
                    _ => throw new InvalidOperationException("Tipo de insumo no válido.")
                };

                return update
                    ? Ok(new { message = "Insumo actualizado correctamente." })
                    : BadRequest(new { status = 400, message = "No se pudo actualizar sel insumo." });
            }
            catch (SupplyException e)
            {
                return BadRequest(new { status = 400, message = e.Message });
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { status = 400, message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
        }

        private async Task<IActionResult> HandleAdd<TDto, TCustomException>(TDto dto, Func<TDto, Task<bool>> addFunc,
            string successMessage) where TCustomException : SupplyException
        {
            try
            {
                var result = await addFunc(dto);

                if (result)
                    return Ok(new { message = successMessage });
                else
                    return BadRequest(new { status = 400, message = "No se pudo crear el registro." });
            }
            catch (TCustomException e)
            {
                return BadRequest(new { status = 400, message = e.Message });
            }
            catch (SupplyException e)
            {
                return BadRequest(new { status = 400, message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
        }

        private object ResolveService(TypeSupply type)
        {
            return type switch
            {
                TypeSupply.Profile => _profileService,
                TypeSupply.Glass => _glassService,
                TypeSupply.Accessory => _accessoryService,
                _ => throw new InvalidOperationException("Tipo de insumo no válido.")
            };
        }

        [HttpGet("insumos")]
        [Authorize]
        public async Task<IActionResult> GetSupplies([FromQuery] string? type = null)
        {
            try
            {
                List<SupplyListItemDto> result = new();

                if (!string.IsNullOrEmpty(type))
                {
                    if (!Enum.TryParse<TypeSupply>(type, true, out TypeSupply supplyType))
                    {
                        return BadRequest(new { status = 400, message = "Tipo de insumo no válido." });
                    }

                    var items = await GetSuppliesByType(supplyType);
                    result.AddRange(items);
                }
                else
                {
                    var profileItems = await GetSuppliesByType(TypeSupply.Profile);
                    result.AddRange(profileItems);

                    var glassItems = await GetSuppliesByType(TypeSupply.Glass);
                    result.AddRange(glassItems);

                    var accessoryItems = await GetSuppliesByType(TypeSupply.Accessory);
                    result.AddRange(accessoryItems);
                }

                result = result.OrderBy(x => x.nameSupply).ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { status = 500, message = "Error interno del servidor: " + e.Message });
            }
        }

        private async Task<List<SupplyListItemDto>> GetSuppliesByType(TypeSupply type)
        {
            var result = new List<SupplyListItemDto>();

            switch (type)
            {
                case TypeSupply.Profile:
                    var profileSupplies = await _profileService.GetAllAsync();
                    foreach (var supply in profileSupplies)
                    {
                        result.Add(new SupplyListItemDto
                        {
                            codeSupply = supply.codeSupply,
                            nameSupply = supply.nameSupply,
                            descriptionSupply = supply.descriptionSupply,
                            imageUrl = supply.imageUrl,
                            nameSupplier = supply.nameSupplier,
                            priceSupply = supply.priceSupply,
                            type = type.ToString()
                        });
                    }
                    break;
                    
                case TypeSupply.Glass:
                    var glassSupplies = await _glassService.GetAllAsync();
                    foreach (var supply in glassSupplies)
                    {
                        result.Add(new SupplyListItemDto
                        {
                            codeSupply = supply.codeSupply,
                            nameSupply = supply.nameSupply,
                            descriptionSupply = supply.descriptionSupply,
                            imageUrl = supply.imageUrl,
                            nameSupplier = supply.nameSupplier,
                            priceSupply = supply.priceSupply,
                            type = type.ToString()
                        });
                    }
                    break;

                case TypeSupply.Accessory:
                    var accessorySupplies = await _accessoryService.GetAllAsync();
                    foreach (var supply in accessorySupplies)
                    {
                        result.Add(new SupplyListItemDto
                        {
                            codeSupply = supply.codeSupply,
                            nameSupply = supply.nameSupply,
                            descriptionSupply = supply.descriptionSupply,
                            imageUrl = supply.imageUrl,
                            nameSupplier = supply.nameSupplier,
                            priceSupply = supply.priceSupply,
                            type = type.ToString()
                        });
                    }
                    break;
                    
                default:
                    throw new InvalidOperationException("Tipo de insumo no válido.");
            }

            return result;
        }
    }
}
