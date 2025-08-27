using ForParts.DTOs.Invoice;
using ForParts.Exceptions.Invoice;
using ForParts.IService.Invoice;
using ForParts.Service.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForParts.Controllers.Invoice
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("crearFactura")]
        [Authorize]
        public async Task<IActionResult> CrearFactura([FromBody] InvoiceCreateDto dto)
        {
            var result = await _invoiceService.CrearFacturaAsync(dto);
            return CreatedAtAction(nameof(ObtenerFacturaPorId), new { id = result.InvoiceId }, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ObtenerFacturaPorId(int id)
        {
            var result = await _invoiceService.ObtenerFacturaPorIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
