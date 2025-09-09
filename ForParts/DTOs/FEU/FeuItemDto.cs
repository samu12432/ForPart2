using Newtonsoft.Json;

namespace ForParts.DTOs.FEU
{
    public class FeuItemDto
    {
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [JsonProperty("cantidad")]
        public int Cantidad { get; set; }

        [JsonProperty("precio_unitario")]
        public decimal PrecioUnitario { get; set; }

        [JsonProperty("total_item")]
        public decimal TotalItem => Math.Round(Cantidad * PrecioUnitario, 2);
    }
}
