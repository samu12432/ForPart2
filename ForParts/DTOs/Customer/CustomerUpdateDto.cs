using ForParts.Models.Customers;
using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Customer
{
    public class CustomerUpdateDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El identificador es requerido")]
        public string Identificador { get; set; } = string.Empty;

        public string TipoDocumento { get; set; } = "RUT";

        [EmailAddress(ErrorMessage = "Email con formato inválido")]
        public string Email { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public Direccion DireccionFiscal { get; set; } = new();
    }
}