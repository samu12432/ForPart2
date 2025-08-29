//using ForParts.Models.Customer;
using ForParts.Models.Customers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForParts.DTOs.Customer
{
         
        public class CustomerDto
    {
            [Key]
            public int CustomerId { get; set; }

            public string Nombre { get; set; } = string.Empty;
            public string Identificador { get; set; } = string.Empty;
            public string TipoDocumento { get; set; } = "RUT";
            public string Email { get; set; } = string.Empty;
            public string Telefono { get; set; } = string.Empty;

            public Direccion DireccionFiscal { get; set; } = new();
        }
    }


