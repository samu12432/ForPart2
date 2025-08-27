using API_REST_PROYECT.DTOs.Product;
using API_REST_PROYECT.IRepository;
using API_REST_PROYECT.Models.Budget;
using API_REST_PROYECT.Models.Enum;
using API_REST_PROYECT.Models.Products;
using API_REST_PROYECT.Models.Supplies;
using API_REST_PROYECT.Supplies.Supply;
using API_REST_PROYECT.Utils;
using NCalc;

namespace API_REST_PROYECT.Repository
{
    public class CalculadoraPresupuestoRepositorio : ICalculadoraPresupuesto
    {
            private readonly IFormulaRepositorio RepoFormula;

            private readonly ISupplyRepository<Profile> _repoProfile;
            private readonly ISupplyRepository<Glass> _repoGlass;
            private readonly ISupplyRepository<Accessory> _repoAccessory;

            private readonly IStockRepository RepoStock;

            private readonly IWebHostEnvironment env;

            private readonly IHttpContextAccessor httpContext;

            public CalculadoraPresupuestoRepositorio(IFormulaRepositorio repoFormula, IStockRepository repoStock,
                IWebHostEnvironment env, IHttpContextAccessor httpContext, ISupplyRepository<Profile> repoProfile,
                ISupplyRepository<Glass> repoVidrio, ISupplyRepository<Accessory> repoAccesorios)
            {
            RepoFormula = repoFormula;
            RepoStock = repoStock;
            this.env = env;
            this.httpContext = httpContext;
            _repoProfile = repoProfile;
            _repoGlass = repoVidrio;
            _repoAccessory = repoAccesorios;
            }

            //-----------------------VENTANAS
            public ProductoPresupuestado CalcularPresupuestoVentanaGala(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoVentanaGalaCR(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public async Task<ProductoPresupuestado> CalcularPresupuestoVentanaProbba(ProductBudgetDto prodDto)
            {
                var insumos = new List<BudgetedSupply>();
                decimal precioProducto = 0;
                string[] codigosDePerfil = { "90020", "90028", "90027", "90021", "90025" };
                foreach (string codigo in codigosDePerfil)
                {
                    Profile perfil = await _repoProfile.GetByCodeAsync(codigo);
                    Formula formula = RepoFormula.GetFormula(codigo, prodDto.Serie.ToString(), prodDto.Tipo.ToString(), "Largo");
                    decimal largo = EvaluarFormula(formula.Expresion, prodDto.Ancho, prodDto.Alto);
                    int cantidad = codigo switch
                    {
                        "90025" => 4,
                        "90020" => 2,
                        "90028" => 2,
                        "90027" => 2,
                        "90021" => 2
                    };
                    bool tieneSuficienteStock = TieneStockDisponible(codigo, prodDto.Color, largo, cantidad);
                    bool faltante = false;
                    if (!tieneSuficienteStock)
                    {
                        faltante = true;
                    }
                    BudgetedSupply perfilInsumo = CalcularPerfil(largo, cantidad, perfil.priceSupply, perfil.weigthMetro, codigo, faltante);
                    insumos.Add(perfilInsumo);
                    precioProducto += perfilInsumo.Subtotal;
                }
                //Calculo del vidrio
                BudgetedSupply vidrioInsumo = CalcularVidrio(prodDto.TipoVidrio, prodDto.EspesorVidrio, prodDto.Serie, prodDto.Tipo, prodDto.Ancho, prodDto.Alto);
                insumos.Add(vidrioInsumo);
                precioProducto += vidrioInsumo.Subtotal;
                // Recargo 14% si es negro o blanco
                precioProducto = AplicarRecargoPorColor(prodDto.Color, precioProducto);

                return new ProductoPresupuestado
                {
                    Nombre = prodDto.Nombre,
                    Ancho = prodDto.Ancho,
                    Alto = prodDto.Alto,
                    Color = prodDto.Color,
                    Cantidad = prodDto.Cantidad,
                    Tipo = prodDto.Tipo,
                    Serie = prodDto.Serie,
                    PrecioUnitario = precioProducto,
                    PrecioTotal = precioProducto * prodDto.Cantidad,
                    InsumosUtilizados = insumos
                };
            }

            public async Task<ProductoPresupuestado> CalcularPresupuestoVentanaS25(ProductBudgetDto prodDto)
            {
                var insumos = new List<BudgetedSupply>();
                decimal precioProducto = 0;
                string[] codigosDePerfil = { "2500", "2528", "2501", "4505", "4507", "4503", "4504" };

                foreach (string codigo in codigosDePerfil)
                {
                    Profile perfil = await _repoProfile.GetByCodeAsync(codigo);

                    Formula formula = RepoFormula.GetFormula(codigo, prodDto.Serie.ToString(), prodDto.Tipo.ToString(), "Largo");

                    decimal largo = EvaluarFormula(formula.Expresion, prodDto.Ancho, prodDto.Alto);
                    int cantidad = codigo switch
                    {
                        "2501" => 2,
                        "4503" => 1,
                        "4504" => 1,
                        "2500" => 2,
                        "2528" => 2,
                        "4505" => 2,
                        "4507" => 2
                    };
                    bool tieneSuficienteStock = TieneStockDisponible(codigo, prodDto.Color, largo, cantidad);
                    bool faltante = false;
                    if (!tieneSuficienteStock)
                    {
                        faltante = true;
                    }
                    BudgetedSupply perfilInsumo = CalcularPerfil(largo, cantidad, perfil.priceSupply, perfil.weigthMetro, codigo, faltante);
                    insumos.Add(perfilInsumo);
                    precioProducto += perfilInsumo.Subtotal;
                }

                //Calculo del vidrio
                BudgetedSupply vidrioInsumo = CalcularVidrio(prodDto.TipoVidrio, prodDto.EspesorVidrio, prodDto.Serie, prodDto.Tipo, prodDto.Ancho, prodDto.Alto);
                insumos.Add(vidrioInsumo);
                precioProducto += vidrioInsumo.Subtotal;

                // Recargo 14% si es negro o blanco
                precioProducto = AplicarRecargoPorColor(prodDto.Color, precioProducto);

                return new ProductoPresupuestado
                {
                    Nombre = prodDto.Nombre,
                    Ancho = prodDto.Ancho,
                    Alto = prodDto.Alto,
                    Color = prodDto.Color,
                    Cantidad = prodDto.Cantidad,
                    Tipo = prodDto.Tipo,
                    Serie = prodDto.Serie,
                    PrecioUnitario = precioProducto,
                    PrecioTotal = precioProducto * prodDto.Cantidad,
                    InsumosUtilizados = insumos
                };
            }

            public Task<ProductoPresupuestado> CalcularPresupuestoVentanaSumma(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }



            public async Task<ProductoPresupuestado> CalcularPresupuestoVentanaS20(ProductBudgetDto prodDto)
            {
                var insumos = new List<BudgetedSupply>();
                decimal precioProducto = 0;
                string[] codigosDePerfil = { "201", "202", "216", "204", "205" };
                foreach (string codigo in codigosDePerfil)
                {
                    Profile perfil = await _repoProfile.GetByCodeAsync(codigo);
                    Formula formula = RepoFormula.GetFormula(codigo, prodDto.Serie.ToString(), prodDto.Tipo.ToString(), "Largo");
                    decimal largo = EvaluarFormula(formula.Expresion, prodDto.Ancho, prodDto.Alto);
                    int cantidad = codigo switch
                    {
                        "201" => 4,
                        "202" => 2,
                        "216" => 2,
                        "204" => 2,
                        "205" => 2
                    };
                    bool tieneSuficienteStock = TieneStockDisponible(codigo, prodDto.Color, largo, cantidad);
                    bool faltante = false;
                    if (!tieneSuficienteStock)
                    {
                        faltante = true;
                    }
                    //Calculo del perfil
                    BudgetedSupply perfilInsumo = CalcularPerfil(largo, cantidad, perfil.priceSupply, perfil.weigthMetro, codigo, faltante);
                    insumos.Add(perfilInsumo);
                    precioProducto += perfilInsumo.Subtotal;
                }

                //Calculo del vidrio
                BudgetedSupply vidrioInsumo = CalcularVidrio(prodDto.TipoVidrio, prodDto.EspesorVidrio, prodDto.Serie, prodDto.Tipo, prodDto.Ancho, prodDto.Alto);
                insumos.Add(vidrioInsumo);
                precioProducto += vidrioInsumo.Subtotal;

                // Recargo 14% si es negro o blanco
                precioProducto = AplicarRecargoPorColor(prodDto.Color, precioProducto);

                return new ProductoPresupuestado
                {
                    Nombre = prodDto.Nombre,
                    Ancho = prodDto.Ancho,
                    Alto = prodDto.Alto,
                    Color = prodDto.Color,
                    Cantidad = prodDto.Cantidad,
                    Tipo = prodDto.Tipo,
                    Serie = prodDto.Serie,
                    PrecioUnitario = precioProducto,
                    PrecioTotal = precioProducto * prodDto.Cantidad,
                    InsumosUtilizados = insumos
                };


            }

            public BudgetedSupply CalcularPerfil(decimal largoEnCm, int cantidad, decimal precioPorKg, decimal pesoPorMetro, string codigo, bool faltante)
            {
                decimal largoEnMetros = largoEnCm / 100;
                decimal totalMetros = largoEnMetros * cantidad;
                decimal subtotal = Math.Round(totalMetros * pesoPorMetro * precioPorKg, 2);

                string urlImagen = ImagenHelper.ObtenerUrl(codigo, httpContext.HttpContext.Request, env);

                return new BudgetedSupply
                {
                    CodigoInsumo = codigo,
                    TipoInsumo = "Perfil",
                    MedidaPorUnidad = largoEnCm.ToString("0.00"),
                    Cantidad = cantidad,
                    PrecioUnitario = precioPorKg,
                    Subtotal = subtotal,
                    FaltanteStock = faltante,
                    ImagenUrl = urlImagen

                };
            }

            public BudgetedSupply CalcularVidrio(GlassType tipoVidrio, string espesor, SerieProfile serie,
                 ProductCategory tipo, decimal ancho, decimal alto)
            {
                // Obtener el vidrio según tipo y espesor
                Glass vidrio = _repoGlass.GetVidrioByTipo(tipoVidrio, espesor); //CAMBIAR LOGICA DE BUSQUEDA
            if (vidrio == null)
                    throw new Exception($"No se encontró el vidrio '{tipoVidrio}' con espesor '{espesor}'.");

                // Obtener fórmulas de cálculo de medidas del vidrio
                Formula formulaAlto = RepoFormula.GetFormula(tipoVidrio.ToString(), serie.ToString(), tipo.ToString(), "Alto");
                Formula formulaAncho = RepoFormula.GetFormula(tipoVidrio.ToString(), serie.ToString(), tipo.ToString(), "Ancho");

                if (formulaAlto == null || formulaAncho == null)
                    throw new Exception($"No se encontró fórmula para el vidrio '{tipoVidrio}' en la serie '{serie}' y tipo '{tipo}'.");

                // Evaluar las fórmulas para obtener las medidas finales del vidrio (en cm)
                decimal altoCalculado = EvaluarFormula(formulaAlto.Expresion, 0, alto);
                decimal anchoCalculado = EvaluarFormula(formulaAncho.Expresion, ancho, 0);

                // Convertir medidas de cm a m para cálculo de precio
                decimal altoEnMetros = altoCalculado / 100;
                decimal anchoEnMetros = anchoCalculado / 100;

                // Calcular el subtotal (precio por m²) con redondeo
                decimal subtotal = Math.Round(anchoEnMetros * altoEnMetros * vidrio.priceSupply, 2);

                string urlImagen = ImagenHelper.ObtenerUrl(vidrio.nameSupply, httpContext.HttpContext.Request, env);//LOGICA

                return new BudgetedSupply
                {
                    CodigoInsumo = vidrio.nameSupply,
                    TipoInsumo = "Vidrio",
                    MedidaPorUnidad = $"{anchoCalculado:0.00} x {altoCalculado:0.00}",
                    Cantidad = 2, // Siempre 2 unidades
                    PrecioUnitario = vidrio.priceSupply,
                    Subtotal = subtotal
                };
            }
            public decimal AplicarRecargoPorColor(string color, decimal precioBase)
            {
                string[] coloresConRecargo = new[] { "negro", "blanco" };
                string colorNormalizado = color.Trim().ToLower();

                if (coloresConRecargo.Contains(colorNormalizado))
                {
                    precioBase *= 1.14m;
                }

                return precioBase;
            }


            //-----------------------PUERTAS
            public ProductoPresupuestado CalcularPresupuestoPuertaS30(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoPuertaMecal30(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoPuertaProbba(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoPuertaGala(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoPuertaSumma(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }
            //-----------------------BATIENTE
            public ProductoPresupuestado CalcularPresupuestoBatienteS30(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoBatienteMecal30(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoBatienteProbba(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoBatienteGala(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoBatienteSumma(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }
            //-----------------------TABAQUERA
            public ProductoPresupuestado CalcularPresupuestoTabaqueraS30(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoTabaqueraMecal30(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoTabaqueraProbba(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoTabaqueraGala(ProductBudgetDto prodDto)
            {   
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoTabaqueraSumma(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }
            //-----------------------PROYECTANTE
            public ProductoPresupuestado CalcularPresupuestoProyectanteS30(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoProyectanteMecal30(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoProyectanteProbba(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoProyectanteGala(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }

            public ProductoPresupuestado CalcularPresupuestoProyectanteSumma(ProductBudgetDto prodDto)
            {
                throw new NotImplementedException();
            }
            private decimal EvaluarFormula(string expresion, decimal ancho, decimal alto)
            {
                var formula = new Expression(expresion);
                formula.Parameters["Ancho"] = (double)ancho;
                formula.Parameters["Alto"] = (double)alto;

                var resultado = formula.Evaluate();
                return Convert.ToDecimal(resultado);
            }
            //public bool o
            public bool TieneStockDisponible(string nombre, string color, decimal largoNecesario, int cantidadRequerida)
            {
                // Paso 1: Buscar todos los perfiles que cumplen con los criterios
                List<Profile> perfilesDisponibles = RepoStock.BuscarPerfilesDisponibles(nombre, color, largoNecesario);

                // Paso 2: Sumar stock disponible para cada uno
                int stockTotalDisponible = 0;

                foreach (var perfil in perfilesDisponibles)
                {
                    int stock = RepoStock.ConsultarCantidadDisponible(perfil.idSupply);
                    stockTotalDisponible += stock;

                    // Si ya alcanzamos la cantidad requerida, podemos salir antes
                    if (stockTotalDisponible >= cantidadRequerida)
                        return true;
                }

                // Paso 3: Verificar si alcanzó o no
                return stockTotalDisponible >= cantidadRequerida;
            }

        ProductoPresupuestado ICalculadoraPresupuesto.CalcularPresupuestoVentanaS20(ProductBudgetDto dto)
        {
            throw new NotImplementedException();
        }

        ProductoPresupuestado ICalculadoraPresupuesto.CalcularPresupuestoVentanaS25(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        Task<ProductoPresupuestado> ICalculadoraPresupuesto.CalcularPresupuestoVentanaGalaCR(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        Task<ProductoPresupuestado> ICalculadoraPresupuesto.CalcularPresupuestoVentanaGala(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        ProductoPresupuestado ICalculadoraPresupuesto.CalcularPresupuestoVentanaSumma(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }
    }
}
