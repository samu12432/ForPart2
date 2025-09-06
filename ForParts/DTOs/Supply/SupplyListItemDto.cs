using ForParts.Models.Enums;

namespace ForParts.DTOs.Supply
{
    public class SupplyListItemDto
    {
        public string codeSupply { get; set; } = string.Empty;
        public string nameSupply { get; set; } = string.Empty;
        public string descriptionSupply { get; set; } = string.Empty;
        public string? imageUrl { get; set; } = string.Empty;
        public string nameSupplier { get; set; } = string.Empty;
        public decimal priceSupply { get; set; }
        public string type { get; set; } = string.Empty;
    }
}