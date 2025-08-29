using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.SqlServer.Management.Sdk.Sfc.OrderBy;
//using ForParts.Models.Customers;

namespace ForParts.Models.Customers
{
    [Table("Customers")]
    public class Customer
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
