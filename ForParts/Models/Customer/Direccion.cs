namespace ForParts.Models.Customer
{
    public class Direccion
    {
        public string Calle { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string Pais { get; set; } = "Uruguay";
    }
}
