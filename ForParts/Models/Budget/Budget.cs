using ForParts.Models.Client;
using ForParts.Models.Enum;
using ForParts.Models.Products;

namespace ForParts.Models.Budget
{
    public class Budget
    {
        public int Id { get; set; }
        public int idClient { get; set; }
        public Customer customer { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public StateBudget Estado { get; set; }
        public decimal PrecioTotal { get; set; }
        public List<ProductoPresupuestado> Productos { get; set; }
    }
}
