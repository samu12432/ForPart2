using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForParts.Models.Supply
{
    [Table("Stock")]
    public class Stock
    {
        [Key]
        public int idStock { get; set; }
        public string codeSupply { get; set; } = string.Empty;

        public int SupplyId {  get; set; }
        public Supply Supply { get; set; } = null!; 
        public int stockQuantity { get; set; }
        public DateTime stockCreate { get; set; }
        public DateTime stockUpdate { get; set; }

        //Atributo para la eliminacion de stock
        public bool IsActive { get; set; } = true;

        public Stock()
        {
            stockCreate = DateTime.Now;
            stockUpdate = DateTime.Now;
        }

        public Stock(string codeSupply, int stockQuantity)
        {
            this.codeSupply = codeSupply;
            this.stockQuantity = stockQuantity;
            stockCreate = DateTime.Now;
            stockUpdate = DateTime.Now;
        }

        public void UpdateQuantity(int newQuantity)
        {
            stockQuantity = newQuantity;
            stockUpdate = DateTime.Now;
        }

        public void changeState()
        {
            this.IsActive = IsActive!;
            stockUpdate = DateTime.Now;
        }
    }
}
