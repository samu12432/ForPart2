using API_REST_PROYECT.DTOs.Budget;
using API_REST_PROYECT.Exceptions.Budget;
using API_REST_PROYECT.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API_REST_PROYECT.Controllers.BudgetC
{
    public class BudgetController : Controller
    {
        [Route("api/[controller]")]
        [ApiController]

        public class PresupuestoController : Controller
        {
            private readonly IBudgetService _budgetService;

            public PresupuestoController(IBudgetService budgetService)
            {
                _budgetService = budgetService;
            }

            [HttpPost("crearPresupuesto")]
            public IActionResult Crear([FromBody] BudgetCreateDto dto)
            {
                //Antes de ingresar a logica, valida los ModelState
                try
                {
                    var presupuesto = _budgetService.CreateBudgetAsync(dto);
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

        }
    }
}
