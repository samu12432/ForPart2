using ForParts.DTOs.FEU;
using ForParts.Exceptions.Invoice;
using ForParts.IService.FEU;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ForParts.Controllers.FEU
{
    public class FacturacionFEUController : Controller
    {
        private readonly IFeuService _feuService;

        public FacturacionFEUController(IFeuService feuService)
        {
            _feuService = feuService;
        }

        /// <summary>
        /// Emite una factura electrónica en ambiente de test usando la API de FEU.
        /// </summary>
        /// <param name="id">ID de la factura a emitir</param>
        /// <returns>Resultado de la emisión</returns>
        [HttpPost("emitirFacturaFeu")]
        [Authorize]
        public async Task<ActionResult<FeuResponseDto>> EmitirFactura(int id)
        {
            try
            {
                var respuesta = await _feuService.EmitirFacturaAsync(id);

                if (!respuesta.Success)
                    return BadRequest(new
                    {
                        mensaje = respuesta.Message,
                        detalle = respuesta.Raw
                    });

                return Ok(respuesta);
            }
            catch (InvoiceException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error inesperado al emitir factura en FEU.",
                    detalle = ex.Message
                });
            }
        }
    }
}
