namespace ForParts.Models.Supply
{
    public class Accessory : Supply
    {
        public string descriptionAccessory { get; set; }

        public Accessory() : base() { }
        public Accessory(int idSupply, string codeSupply, string nameSupply, string descriptionSupply, string nameSupplier, decimal priceSupply,
            string descriptionAccessory, string imageUrl)
            : base(idSupply, codeSupply, nameSupply, descriptionSupply, imageUrl, nameSupplier, priceSupply)
        {
            this.descriptionAccessory = descriptionAccessory;
        }

    }
}
