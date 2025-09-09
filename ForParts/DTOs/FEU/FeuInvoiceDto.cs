using Newtonsoft.Json;

namespace ForParts.DTOs.FEU
{
    public class FeuInvoiceDto
    {
        [JsonProperty("tipo_comprobante")]
        public string TipoComprobante { get; set; } = "111"; // e-Factura

        [JsonProperty("fecha_emision")]
        public string FechaEmision { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        [JsonProperty("rut_emisor")]
        public string RutEmisor { get; set; } = "214198620015";

        [JsonProperty("sucursal")]
        public string Sucursal { get; set; } = "1";

        [JsonProperty("rut_receptor")]
        public string RutReceptor { get; set; } = string.Empty;

        [JsonProperty("nombre_receptor")]
        public string NombreReceptor { get; set; } = string.Empty;

        [JsonProperty("domicilio_receptor")]
        public string DomicilioReceptor { get; set; } = string.Empty;

        [JsonProperty("moneda")]
        public string Moneda { get; set; } = "UYU";

        [JsonProperty("forma_pago")]
        public string FormaPago { get; set; } = "Contado"; // o "Crédito"

        [JsonProperty("items")]
        public List<FeuItemDto> Items { get; set; } = new();

        [JsonProperty("total")]
        public decimal Total => Items.Sum(i => i.TotalItem);
    }
}
