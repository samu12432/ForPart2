using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ForParts.Validation
{
    public class ValidationFormatter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext x)
        {
            if (!x.ModelState.IsValid)
            {
                var errores = x.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var httpContext = x.HttpContext;

                x.Result = new JsonResult(new
                {
                    status = 400,
                    message = string.Join(" | ", errores)
                })
                {
                    StatusCode = 400
                };
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}


/*{Cambiamos la forma en la que es la salida de los errores cuando se utiliza MODELSTATE.ISVALID en cada controller

Pasamos de esto


"type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
"title": "One or more validation errors occurred.",
"status": 400,
"errors": {
"name": [
"Es necesario ingresar el nombre y apellido."
]
},
"traceId": "00-1b1a34f249619f4b6cfb6a1d39f20ceb-e41c2be30461a411-00"
}
}

A esto

"status": 400,
"message": "Es necesario ingresar el nombre y apellido.",*/

