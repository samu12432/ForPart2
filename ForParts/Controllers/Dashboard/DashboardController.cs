using ForParts.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForParts.Controllers.Dashboard
{
    public class DashboardController : ControllerBase
    {
        private readonly ContextDb _context;

        public DashboardController(ContextDb context)
        {
            _context = context;
        }

        [HttpGet("resumen")]
        public async Task<IActionResult> GetResumen()
        {
            try
            {
                int contactos = await _context.Customers.CountAsync();
                int productos = await _context.Products.CountAsync();
                int insumos = await _context.Supplies.CountAsync();

                var resumen = new
                {
                    contactos,
                    productos,
                    insumos
                };

                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el resumen del dashboard", error = ex.Message });
            }
        }
    }
}