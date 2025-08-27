using API_REST_PROYECT.Models.Enum;
using API_REST_PROYECT.Models.Products;

namespace API_REST_PROYECT.Models.Budget
{
    public class Formula
    {
        public int Id { get; set; }
        public string CodigoInsumo { get; set; } = string.Empty;// Ej: 201
        public string TipoInsumo { get; set; } = string.Empty;// Ej: Perfil, Vidrio
        public SerieProfile SeriePerfil { get; set; } // Ej: Serie20
        public ProductCategory TipoProducto { get; set; } // Ej: Ventana, Puerta
        public string Expresion { get; set; } = string.Empty;// Ej: "(Ancho / 2) - 7"
        public string Descripcion { get; set; } = string.Empty;
    }
}
