using ForParts.DTOs.Auth;
using ForParts.Exceptions.Auth;
using ForParts.IService.Auth;
using ForParts.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace ForParts.Controllers.Auth
{
   
    public class AuthController : Controller
    {
        private readonly IServiceAuth _authService;

        public AuthController(IServiceAuth authService)
        {
            _authService = authService;
        }
        

        [HttpPost("registroUsuario")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto dto)
        {
            //var stopwatch = Stopwatch.StartNew(); // Inicia el cronómetro

            //Antes de ejecutar la funcion, se realiza la validacion de MODELSTATE correspondiente al Dto
            try
            {
                var result = await _authService.RegisterUser(dto);
                if (!result)
                    return Unauthorized("El correo ya está registrado."); //Codigo 401


               // stopwatch.Stop(); // Detiene el cronómetro
               // var tiempoRespuesta = stopwatch.ElapsedMilliseconds; // Tiempo en ms

              //  return Ok(new { Mensaje = "Usuario registrado correctamente", TiempoMs = tiempoRespuesta });

               return Ok("Usuario registrado correctamente."); //Codigo 200
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { satus = 400, message = e.Message });
            }
            catch (EmailException e)
            {
                return BadRequest(new { satus = 400, message = e.Message });
            }
            catch (UserNameException e)
            {
                return BadRequest(new { satus = 400, message = e.Message });
            }

        }
        [AllowAnonymous]
        [HttpGet("/api/auth/confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return BadRequest("Token requerido.");
            var ok = await _authService.ConfirmEmailAsync(token);
            if (!ok) return BadRequest("Token inválido o expirado.");

            // Redirigí a una vista del front si querés mostrar un mensaje
            return Ok("Email confirmado");
            // return Redirect("https://front.edaberturas.lat/confirmacion-exitosa");
        }


        [AllowAnonymous]
        [HttpPost("/api/auth/confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfrimEmailDto token)
        {
            try
            {
                var result = await _authService.ConfirmEmailAsync(token.token); // valida el token y activa el usuario

                if (!result)
                    return BadRequest("Token es invalido o se expiro");

                return Ok("Email confirmado.");
            }
            catch (Exception e)
            {
                return BadRequest(new { status = 500, message = e.Message });
            }

        }


        [HttpPost("loginUsuario")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto dto)
        {
            //Antes de ejecutar la funcion, se realiza la validacion de MODELSTATE correspondiente al Dto
            try
            {
                (string? token, User user) = await _authService.LoginUser(dto);
                
                if(user != null)
                {
                    user.passwordHash = user.passwordSalt = null;
                }

                if (string.IsNullOrEmpty(token))
                    return Unauthorized("Credenciales incorretas"); //Codigo 401

                return Ok(new { token, user }); //Codigo 200
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { status = 500, message = e.Message });
            }
            catch (EmailException e)
            {
                return BadRequest(new { status = 403, message = e.Message });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
