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
                dynamic service = ResolveService(dto.type);
                bool deleted = await service.DeleteByCodeAsync(dto.codeSupply);

                return deleted
                    ? Ok("Eliminado correctamente.")
                    : BadRequest(new { status = 400, message = "No se pudo eliminar el insumo." });
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
        public async Task<IActionResult> UpdateSupply([FromBody] EditSupplyDto dto)
        {
            try
            {
                dynamic service = ResolveService(dto.type);
                bool update = await service.updateSupply(dto);

                return update
                    ? Ok("Insumo actualizado correctamente.")
                    : BadRequest(new { status = 400, message = "No se pudo actualizar sel insumo." });
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
        public async Task<IActionResult> UpdateImageSupply([FromForm] EditImageSupplyDto dto)
        {
            try
            {
                dynamic service = ResolveService(dto.type);
                bool update = await service.updateImageSupply(dto);

                return update
                    ? Ok("Insumo actualizado correctamente.")
                    : BadRequest(new { status = 400, message = "No se pudo actualizar sel insumo." });
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
        public async Task<IActionResult> UpdateSupply([FromBody] EditPriceSupplyDto dto)
        {
            try
            {
                dynamic service = ResolveService(dto.type);
                bool update = await service.updateSupply(dto);

                return update
                    ? Ok("Insumo actualizado correctamente.")
                    : BadRequest(new { status = 400, message = "No se pudo actualizar sel insumo." });
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
                    return Ok(successMessage);
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
    }
}
