using ForParts.DTOs.Budget;
using ForParts.Exceptions.Budget;
using ForParts.IService.Buget;
using ForParts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForParts.Controllers.Budget
{
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly IBudgetPdfService _pdf;
        public BudgetController(IBudgetService budgetService, IBudgetPdfService pdf)
        {
            _pdf = pdf;
            _budgetService = budgetService;
        }

        [HttpPost("crearPresupuesto")]
        [Authorize]
        public async Task<IActionResult> Crear([FromBody] BudgetCreateDto dto)
        {
            //Antes de ingresar a logica, valida los ModelState
            try
            {
                var presupuesto = await _budgetService.CreateBudgetAsync(dto);
                return Ok(presupuesto);
            }
            catch (BudgetException ex)
            {
                return BadRequest(new { state = 400, mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GetPdf(int id, CancellationToken ct)
        {
            var b = await _budgetService.FindByIdAsync(id, ct);
            if (b == null) return NotFound();

            var pdf = _pdf.Generate(b);

            // Permite que front/Swagger lean el nombre del archivo
            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");

            var fileName = $"Presupuesto-{id}.pdf";
            return File(pdf, "application/pdf", fileName);
        }


    }
}
