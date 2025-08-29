using ForParts.DTO.Product;
using ForParts.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Product
{
    public class ProductBudgetDto
    {
        public string Name { get; set; }
        public decimal Width { get; set; }
        public decimal Heigth { get; set; }
        public string Color { get; set; }
        public int amount { get; set; }

        public string GlassThickness { get; set; }
        public GlassType GlassType { get; set; }

        public ProductType TypeProduct { get; set; }
        public SerieProfile Serie { get; set; }
    }
}
