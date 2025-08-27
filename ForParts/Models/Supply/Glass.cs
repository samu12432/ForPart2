using ForParts.Models.Enums;

namespace ForParts.Models.Supply
{
    public class Glass : Supply
    {
        public decimal glassThickness { get; set; }
        public decimal glassLength { get; set; }
        public decimal glassWidth { get; set; }
        public GlassType glassType { get; set; }

        public Glass() : base() { }

        public Glass(int idSupply, string codeSupply, string nameSupply, string descriptionSupply, string imageUrl, string nameSupplier, decimal priceSupply,
                     decimal glassThickness, decimal glassLength, decimal glassWidth, GlassType glassType)
            : base(idSupply, codeSupply, nameSupply, descriptionSupply, imageUrl, nameSupplier, priceSupply)
        {
            this.glassThickness = glassThickness;
            this.glassLength = glassLength;
            this.glassWidth = glassWidth;
            this.glassType = glassType;
        }
    }
}
