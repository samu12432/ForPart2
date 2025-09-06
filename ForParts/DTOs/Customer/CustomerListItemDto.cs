namespace ForParts.DTOs.Customer
{
    public class CustomerListItemDto
    {
        public int CustomerId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Identificador { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}