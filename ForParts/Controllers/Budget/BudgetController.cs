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

        public BudgetController(IBudgetService budgetService)
        {
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


    }
}
