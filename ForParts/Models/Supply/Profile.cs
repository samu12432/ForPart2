namespace ForParts.Models.Supply
{
    public class Profile : Supply
    {
        public decimal profileWeigth { get; set; } //Peso
        public decimal profileHeigth { get; set; }//Largo
        public decimal weigthMetro { get; set; } //PorMetro
        public string profileColor { get; set; } = string.Empty; //Color

        public Profile() : base() { }

        public Profile(int idSupply, string codeSupply, string nameSupply, string descriptionSupply, string imageUrl, string nameSupplier, decimal priceSupply,
                      decimal profileWeigth, decimal profileHeigth, decimal weigthMetro, string profileColor)
            : base(idSupply, codeSupply, nameSupply, descriptionSupply, imageUrl, nameSupplier, priceSupply)
        {
            this.profileWeigth = profileWeigth;
            this.profileHeigth = profileHeigth;
            this.weigthMetro = weigthMetro;
            this.profileColor = profileColor;
        }
    }
}
