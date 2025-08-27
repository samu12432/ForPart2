using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Es necesario ingresar el nombre y apellido.")]
        [StringLength(100)]
        public string name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Es necesario ingresar un correo.")]
        [EmailAddress(ErrorMessage = "Formato de correo invalido.")]
        public string userEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Es necesario ingresar un nombre de usuario.")]
        [StringLength(100)]
        public string userName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^09\d{7}$", ErrorMessage = "El número de teléfono debe ser uruguayo y con formato 09xxxxxx")]
        public string phoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Es necesario ingresar una contraseña.")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Largo de la contraseña mayor a 6 y menor a 10")]
        public string password { get; set; } = string.Empty;
    }
}
