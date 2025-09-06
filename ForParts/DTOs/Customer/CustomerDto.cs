using ForParts.Models.Customers;
using System.Text.Json.Serialization;
using ForParts.Converters;

namespace ForParts.DTOs.Customer
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        
        public string Nombre { get; set; } = string.Empty;
        
        public string? Identificador { get; set; }
        
        public string? TipoDocumento { get; set; }
        
        public string? Email { get; set; }
        
        public string? Telefono { get; set; }

        [JsonConverter(typeof(DireccionJsonConverter))]
        public Direccion? DireccionFiscal { get; set; }
    }
}


