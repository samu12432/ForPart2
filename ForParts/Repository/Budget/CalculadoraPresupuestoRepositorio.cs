using ForParts.DTOs.Product;
using ForParts.IRepository;
using ForParts.IRepository.Budget;
using ForParts.IRepository.Supply;
using ForParts.Models.Budgetes;
using ForParts.Models.Enums;
using ForParts.Models.Product;
using ForParts.Models.Supply;
using System.Text.Json;

//using ForParts.Supplies.Supply;
using ForParts.Utils;
using NCalc;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ForParts.Repository.Budget
{
    public class CalculadoraPresupuestoRepositorio : IBudgetCalculator
    {
        private readonly IFormulaRepository RepoFormula;

        private readonly ISupplyRepository<Profile> _repoProfile;
        private readonly ISupplyRepository<Glass> _repoGlass;
        private readonly ISupplyRepository<Accessory> _repoAccessory;

        private readonly IStockRepository RepoStock;

        private readonly IWebHostEnvironment env;

        private readonly IHttpContextAccessor httpContext;

        public CalculadoraPresupuestoRepositorio(IFormulaRepository repoFormula, IStockRepository repoStock,
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
        public async Task<BudgetedProduct> CalcularPresupuestoVentanaGala(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoVentanaGalaCR(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoVentanaProbba(ProductBudgetDto prodDto)
        {
            var insumos = new List<BudgetedSupply>();
            decimal precioProducto = 0;
            string[] codigosDePerfil = { "90020", "90028", "90027", "90021", "90025" };
            foreach (string codigo in codigosDePerfil)
            {
                Profile perfil = await _repoProfile.GetSupplyByCode(codigo);
                //Para la formula, llega el enum, pero para acceder a la bd. necesitamos el valor INT   
                Formula formula = RepoFormula.GetFormula(codigo, ((int)prodDto.Serie), ((int)prodDto.TypeProduct), "Largo");
                decimal largo = EvaluarFormula(formula.Expresion, prodDto.Width, prodDto.Heigth);
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
            BudgetedSupply vidrioInsumo = CalcularVidrio(prodDto.GlassType, prodDto.GlassThickness, prodDto.Serie, prodDto.TypeProduct, prodDto.Width, prodDto.Heigth);
            insumos.Add(vidrioInsumo);
            precioProducto += vidrioInsumo.Subtotal;
            // Recargo 14% si es negro o blanco
            precioProducto = AplicarRecargoPorColor(prodDto.Color, precioProducto);
            //Calculo de mano de obra
            precioProducto += CalculateLaborCost(prodDto.Width, prodDto.Heigth, prodDto.TypeProduct);
            //Se divide todo al precio del dolar hoy
            decimal precioDolar = await GetUsdUyuVentaAsync();
            precioProducto = precioProducto * precioDolar;
            return new BudgetedProduct
            {
                Name = prodDto.Name,
                Width = prodDto.Width,
                Heigth = prodDto.Heigth,
                Color = prodDto.Color,
                Amount = prodDto.amount,
                ProductType = prodDto.TypeProduct,
                SerieProfile = prodDto.Serie,
                UnitPrice = precioProducto,
                TotalPrice = precioProducto * prodDto.amount,
                SuppliesUsed = insumos
            };
        }

        public async Task<BudgetedProduct> CalcularPresupuestoVentanaS25(ProductBudgetDto prodDto)
        {
            var insumos = new List<BudgetedSupply>();
            decimal precioProducto = 0;
            string[] codigosDePerfil = { "2500", "2528", "2501", "4505", "4507", "4503", "4504" };

            foreach (string codigo in codigosDePerfil)
            {
                Profile perfil = await _repoProfile.GetByCodeAsync(codigo);
                //Para la formula, llega el enum, pero para acceder a la bd. necesitamos el valor INT
                Formula formula = RepoFormula.GetFormula(codigo, ((int)prodDto.Serie), ((int)prodDto.TypeProduct), "Largo");

                decimal largo = EvaluarFormula(formula.Expresion, prodDto.Width, prodDto.Heigth);
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
            BudgetedSupply vidrioInsumo = CalcularVidrio(prodDto.GlassType, prodDto.GlassThickness, prodDto.Serie, prodDto.TypeProduct, prodDto.Width, prodDto.Heigth);
            insumos.Add(vidrioInsumo);
            precioProducto += vidrioInsumo.Subtotal;

            // Recargo 14% si es negro o blanco
            precioProducto = AplicarRecargoPorColor(prodDto.Color, precioProducto);
            //Calculo de mano de obra
            precioProducto += CalculateLaborCost(prodDto.Width, prodDto.Heigth, prodDto.TypeProduct);
            //Se divide todo al precio del dolar hoy
            decimal precioDolar = await GetUsdUyuVentaAsync();
            precioProducto = precioProducto * precioDolar;
            return new BudgetedProduct
            {
                Name = prodDto.Name,
                Width = prodDto.Width,
                Heigth = prodDto.Heigth,
                Color = prodDto.Color,
                Amount = prodDto.amount,
                ProductType = prodDto.TypeProduct,
                SerieProfile = prodDto.Serie,
                UnitPrice = precioProducto,
                TotalPrice = precioProducto * prodDto.amount,
                SuppliesUsed = insumos
            };
        }

        public Task<BudgetedProduct> CalcularPresupuestoVentanaSumma(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }



        public async Task<BudgetedProduct> CalcularPresupuestoVentanaS20(ProductBudgetDto prodDto)
        {

            var insumos = new List<BudgetedSupply>();
            decimal precioProducto = 0;
            string[] codigosDePerfil = { "201", "202", "216", "204", "205" };
            //string[] codigosDePerfil = { "201" };

            foreach (string codigo in codigosDePerfil)
            {
                Profile perfil = await _repoProfile.GetByCodeAsync(codigo);

                //Para la formula, llega el enum, pero para acceder a la bd. necesitamos el valor INT
                Formula formula = RepoFormula.GetFormula(codigo, ((int)prodDto.Serie), ((int)prodDto.TypeProduct), "Largo");
                decimal largo = EvaluarFormula(formula.Expresion, prodDto.Width, prodDto.Heigth);
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
            BudgetedSupply vidrioInsumo = CalcularVidrio(prodDto.GlassType, prodDto.GlassThickness, prodDto.Serie, prodDto.TypeProduct, prodDto.Width, prodDto.Heigth);
            insumos.Add(vidrioInsumo);
            //precioProducto += vidrioInsumo.Subtotal;


            // Recargo 14% si es negro o blanco
            precioProducto = AplicarRecargoPorColor(prodDto.Color, precioProducto);
            //Calculo de mano de obra
            precioProducto += CalculateLaborCost(prodDto.Width, prodDto.Heigth, prodDto.TypeProduct);
            //Se divide todo al precio del dolar hoy
            decimal precioDolar = await GetUsdUyuVentaAsync();
            precioProducto = Math.Round(precioProducto * precioDolar, 2, MidpointRounding.AwayFromZero);

            return new BudgetedProduct
            {
                Name = prodDto.Name,
                Width = prodDto.Width,
                Heigth = prodDto.Heigth,
                Color = prodDto.Color,
                Amount = prodDto.amount,
                ProductType = prodDto.TypeProduct,
                SerieProfile = prodDto.Serie,
                UnitPrice = precioProducto,
                TotalPrice = precioProducto * prodDto.amount,
                SuppliesUsed = insumos
            };


        }



        //-----------------------PUERTAS
        public async Task<BudgetedProduct> CalcularPresupuestoPuertaS30(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoPuertaMecal30Interior(ProductBudgetDto prodDto)
        {

            var insumos = new List<BudgetedSupply>();
            decimal precioProducto = 0;
            string[] codigosDePerfil = { "307A", "307", "315", "317", "313" };

            foreach (string codigo in codigosDePerfil)
            {
                Profile perfil;
                if (codigo == "307A")
                {
                    string codigo307 = "307";
                    perfil = await _repoProfile.GetByCodeAsync(codigo307);
                }
                else
                {
                    perfil = await _repoProfile.GetByCodeAsync(codigo);
                }
                //Para la formula, llega el enum, pero para acceder a la bd. necesitamos el valor INT
                Formula formula = RepoFormula.GetFormula(codigo, ((int)prodDto.Serie), ((int)prodDto.TypeProduct), "Largo");
                decimal largo = EvaluarFormula(formula.Expresion, prodDto.Width, prodDto.Heigth);
                int cantidad = codigo switch
                {
                    "307" => 2,
                    "307A" => 1,
                    "317" => 2,
                    "313" => 1,
                    "315" => 1

                };
                bool faltante = false;
                if (codigo == "307A")
                {// se hace esto por que para el perfil 307 se necesitan dos medidas diferentes para la misma abertura
                    string codigo307 = "307";
                    bool tieneSuficienteStock = TieneStockDisponible(codigo307, prodDto.Color, largo, cantidad);
                    if (!tieneSuficienteStock)
                    {
                        faltante = true;
                    }
                    if (perfil == null) continue;
                    BudgetedSupply perfilInsumo = CalcularPerfil(largo, cantidad, perfil.priceSupply, perfil.weigthMetro, codigo, faltante);
                    insumos.Add(perfilInsumo);
                    precioProducto += perfilInsumo.Subtotal;
                }
                else
                {
                    bool tieneSuficienteStock = TieneStockDisponible(codigo, prodDto.Color, largo, cantidad);
                    if (!tieneSuficienteStock)
                    {
                        faltante = true;
                    }
                }
                //Calculo del perfil
                BudgetedSupply perfilInsumo = CalcularPerfil(largo, cantidad, perfil.priceSupply, perfil.weigthMetro, codigo, faltante);
                insumos.Add(perfilInsumo);
                precioProducto += perfilInsumo.Subtotal;
            }

            // Recargo 14% si es negro o blanco
            precioProducto = AplicarRecargoPorColor(prodDto.Color, precioProducto);
            //Calculo de mano de obra
            precioProducto += CalculateLaborCost(prodDto.Width, prodDto.Heigth, prodDto.TypeProduct);
            //Se divide todo al precio del dolar hoy
            decimal precioDolar = await GetUsdUyuVentaAsync();
            precioProducto = Math.Round(precioProducto * precioDolar, 2, MidpointRounding.AwayFromZero);

            return new BudgetedProduct
            {
                Name = prodDto.Name,
                Width = prodDto.Width,
                Heigth = prodDto.Heigth,
                Color = prodDto.Color,
                Amount = prodDto.amount,
                ProductType = prodDto.TypeProduct,
                SerieProfile = prodDto.Serie,
                UnitPrice = precioProducto,
                TotalPrice = precioProducto * prodDto.amount,
                SuppliesUsed = insumos
            };

        }
        public Task<BudgetedProduct> CalcularPresupuestoPuertaMecal30Exterior(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoPuertaProbba(ProductBudgetDto prodDto)
        {
            var insumos = new List<BudgetedSupply>();
            decimal precioProducto = 0;
            string[] codigosDePerfil = { "90000", "90000A", "90008", "90008A", "92486" };

            foreach (string codigo in codigosDePerfil)
            {
                Profile perfil;
                if (codigo == "90000A")
                {
                    string codigo90000 = "90000A";
                    perfil = await _repoProfile.GetByCodeAsync(codigo90000);
                }
                else if (codigo == "90008A")
                {
                    string codigo90008 = "90008A";
                    perfil = await _repoProfile.GetByCodeAsync(codigo90008);

                }
                else
                {
                    perfil = await _repoProfile.GetByCodeAsync(codigo);
                }
                //Para la formula, llega el enum, pero para acceder a la bd. necesitamos el valor INT
                Formula formula = RepoFormula.GetFormula(codigo, ((int)prodDto.Serie), ((int)prodDto.TypeProduct), "Largo");
                decimal largo = EvaluarFormula(formula.Expresion, prodDto.Width, prodDto.Heigth);
                int cantidad = codigo switch
                {
                    "90000" => 1,
                    "90000A" => 2,
                    "90008" => 2,
                    "90008A" => 2,
                    "92486" => 1

                };
                bool faltante = false;
                if (codigo == "90000A")
                {// se hace esto por que para el perfil 90000A se necesitan dos medidas diferentes para la misma abertura
                    string codigo90000 = "90000";
                    bool tieneSuficienteStock = TieneStockDisponible(codigo90000, prodDto.Color, largo, cantidad);
                    if (!tieneSuficienteStock)
                    {
                        faltante = true;
                    }
                }
                else if (codigo == "90008A")
                {// se hace esto por que para el perfil 90000A se necesitan dos medidas diferentes para la misma abertura
                    string codigo90008 = "90008";
                    bool tieneSuficienteStock = TieneStockDisponible(codigo90008, prodDto.Color, largo, cantidad);
                    if (!tieneSuficienteStock)
                    {
                        faltante = true;
                    }
                }
                else
                {
                    bool tieneSuficienteStock = TieneStockDisponible(codigo, prodDto.Color, largo, cantidad);
                    if (!tieneSuficienteStock)
                    {
                        faltante = true;
                    }
                }
                //Calculo del perfil
                BudgetedSupply perfilInsumo = CalcularPerfil(largo, cantidad, perfil.priceSupply, perfil.weigthMetro, codigo, faltante);
                insumos.Add(perfilInsumo);
                precioProducto += perfilInsumo.Subtotal;
            }

            // Recargo 14% si es negro o blanco
            precioProducto = AplicarRecargoPorColor(prodDto.Color, precioProducto);
            //Calculo de mano de obra
            precioProducto += CalculateLaborCost(prodDto.Width, prodDto.Heigth, prodDto.TypeProduct);
            //Se divide todo al precio del dolar hoy
            decimal precioDolar = await GetUsdUyuVentaAsync();
            precioProducto = Math.Round(precioProducto * precioDolar, 2, MidpointRounding.AwayFromZero);

            return new BudgetedProduct
            {
                Name = prodDto.Name,
                Width = prodDto.Width,
                Heigth = prodDto.Heigth,
                Color = prodDto.Color,
                Amount = prodDto.amount,
                ProductType = prodDto.TypeProduct,
                SerieProfile = prodDto.Serie,
                UnitPrice = precioProducto,
                TotalPrice = precioProducto * prodDto.amount,
                SuppliesUsed = insumos
            };

        }

        public async Task<BudgetedProduct> CalcularPresupuestoPuertaGala(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoPuertaSumma(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }
        //-----------------------BATIENTE
        public async Task<BudgetedProduct> CalcularPresupuestoBatienteS30(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoBatienteMecal30(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoBatienteProbba(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoBatienteGala(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoBatienteSumma(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }
        //-----------------------TABAQUERA
        public async Task<BudgetedProduct> CalcularPresupuestoTabaqueraS30(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoTabaqueraMecal30(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoTabaqueraProbba(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoTabaqueraGala(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoTabaqueraSumma(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }
        //-----------------------PROYECTANTE
        public async Task<BudgetedProduct> CalcularPresupuestoProyectanteS30(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoProyectanteMecal30(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoProyectanteProbba(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoProyectanteGala(ProductBudgetDto prodDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BudgetedProduct> CalcularPresupuestoProyectanteSumma(ProductBudgetDto prodDto)
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
        public bool TieneStockDisponible(string codeSupply, string color, decimal largoNecesario, int cantidadRequerida)
        {
            // Paso 1: Buscar todos los perfiles que cumplen con los criterios
            List<Profile> perfilesDisponibles = RepoStock.GetAvailableProfiles(codeSupply, color, largoNecesario);

            // Paso 2: Sumar stock disponible para cada uno
            int stockTotalDisponible = 0;

            foreach (var perfil in perfilesDisponibles)
            {
                int stock = RepoStock.GetAvailableQuantity(perfil.idSupply);
                stockTotalDisponible += stock;

                // Si ya alcanzamos la cantidad requerida, podemos salir antes
                if (stockTotalDisponible >= cantidadRequerida)
                    return true;
            }

            // Paso 3: Verificar si alcanzó o no
            return stockTotalDisponible >= cantidadRequerida;
        }
        public BudgetedSupply CalcularPerfil(decimal largoEnCm, int cantidad, decimal precioPorKg, decimal pesoPorMetro, string codigo, bool faltante)
        {
            decimal largoEnMetros = largoEnCm / 100;
            decimal totalMetros = largoEnMetros * cantidad;
            decimal subtotal = Math.Round(totalMetros * pesoPorMetro * precioPorKg, 2);

            string urlImagen = ImagenHelper.ObtenerUrl(codigo, httpContext.HttpContext.Request, env);


            return new BudgetedSupply
            {
                SupplyCode = codigo,
                TypeSupply = TypeSupply.Profile,
                UnitMeasure = largoEnCm.ToString("0.00"),
                amount = cantidad,
                UnityPrice = precioPorKg,
                Subtotal = subtotal,
                OutOfStock = faltante,
                ImageUrl = urlImagen

            };
        }

        public BudgetedSupply CalcularVidrio(GlassType tipoVidrio, string espesor, SerieProfile serie,
                ProductType tipo, decimal ancho, decimal alto)
        {
            // Obtener el vidrio según tipo y espesor
            Glass vidrio = _repoGlass.GetGlassByType(tipoVidrio, espesor); //CAMBIAR LOGICA DE BUSQUEDA
            if (vidrio == null)
                throw new Exception($"No se encontró el vidrio '{tipoVidrio}' con espesor '{espesor}'.");

            // Obtener fórmulas de cálculo de medidas del vidrio
            //Para la formula, llega el enum, pero para acceder a la bd. necesitamos el valor INT
            Formula formulaAlto = RepoFormula.GetFormula(tipoVidrio.ToString(), ((int)serie), ((int)tipo), "Alto");
            Formula formulaAncho = RepoFormula.GetFormula(tipoVidrio.ToString(), ((int)serie), ((int)tipo), "Ancho");

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

            string urlImagen = ImagenHelper.ObtenerUrl(vidrio.codeSupply, httpContext.HttpContext.Request, env);//LOGICA

            return new BudgetedSupply
            {
                SupplyCode = vidrio.nameSupply,
                TypeSupply = TypeSupply.Glass,
                UnitMeasure = $"{anchoCalculado:0.00} x {altoCalculado:0.00}",
                amount = 2, // Siempre 2 unidades
                UnityPrice = vidrio.priceSupply,
                Subtotal = subtotal,
                ImageUrl = urlImagen,
                OutOfStock = false
            };
        }
        public decimal AplicarRecargoPorColor(string color, decimal precioBase)
        {
            string[] coloresConRecargo = new[] { "negro", "blanco", "madera", "marron", "anolock" };
            string colorNormalizado = color.Trim().ToLower();

            if (coloresConRecargo.Contains(colorNormalizado))
            {
                precioBase *= 1.14m;
            }

            return precioBase;
        }

        public decimal CalculateLaborCost(decimal ancho, decimal alto, ProductType tipoProducto)
        {

            decimal costoBase = 50m;

            if ((alto >= 200 || ancho >= 200) && tipoProducto == ProductType.Ventana)
            {
                costoBase = 87.50m;
            }
            else if ((alto >= 150 || ancho >= 150) && tipoProducto == ProductType.Ventana)
            {
                costoBase = 62.50m;
            }
            else if ((alto < 150 && ancho < 150) && tipoProducto == ProductType.Batiente)
            {
                costoBase = 75m;
            }
            else if ((alto >= 150 || ancho >= 150) && tipoProducto == ProductType.Batiente)
            {
                costoBase = 100m;
            }
            else if (tipoProducto == ProductType.Puerta)
            {
                costoBase = 112.50m;
            }
            return costoBase;

        }
        public static async Task<decimal> GetUsdUyuVentaAsync()
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(6) };
            using var stream = await http.GetStreamAsync("https://uy.dolarapi.com/v1/cotizaciones/usd");
            using var doc = await JsonDocument.ParseAsync(stream);

            if (!doc.RootElement.TryGetProperty("venta", out var ventaProp) ||
                !ventaProp.TryGetDecimal(out var venta))
                throw new Exception("La respuesta no tiene el campo 'venta'.");

            return venta; // tipo de cambio de venta (USD->UYU)
        }


    }



}
