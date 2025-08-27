using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Auth
{
    public class ConfrimEmailDto
    {
        [Required(ErrorMessage = "Se necesita token de validación.")]
        public string token { get; set; } = string.Empty;
    }
}
