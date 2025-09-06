using ForParts.Models.Enums;

namespace ForParts.Models.Supply
{
    public class Accessory : Supply
    {
        public string descriptionAccessory { get; set; }

        public TypeAccessory typeAccessory { get; set; }

        public SerieProfile serie {  get; set; }    

        public string color { get; set; }

        public Accessory() : base() { }
        public Accessory(int idSupply, string codeSupply, string nameSupply, string descriptionSupply, string nameSupplier, decimal priceSupply,
            string descriptionAccessory, string imageUrl, TypeAccessory typeAccesory, SerieProfile serie, string color)
            : base(idSupply, codeSupply, nameSupply, descriptionSupply, imageUrl, nameSupplier, priceSupply)
        {
            this.descriptionAccessory = descriptionAccessory;
            this.typeAccessory = typeAccesory;
            this.serie = serie;
            this.color = color;
        }

    }
}
