using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ForParts.Models.Budgetes;
using ForParts.Models.Product;     // BudgetedProduct
using ForParts.Models.Supply;      // BudgetedSupply
using budgets = ForParts.Models.Budgetes.Budget;

namespace ForParts.Service.Budget
{
    public class BudgetPdfService : IBudgetPdfService
    {
        private readonly CultureInfo _uy = new CultureInfo("es-UY");

        public byte[] Generate(budgets b)
        {
            if (b == null) throw new ArgumentNullException(nameof(b));

            var productos = b.Products?.ToList() ?? new List<BudgetedProduct>();
            var fecha = (b.Date == default ? DateTime.Now : b.Date).ToString("dd/MM/yyyy HH:mm", _uy);

            QuestPDF.Settings.License = LicenseType.Community;

            var bytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    // ===== Header =====
                    page.Header().Element(e =>
                    {
                        e.Row(row =>
                        {
                            row.RelativeItem().Text(txt =>
                            {
                                txt.Span("PRESUPUESTO").FontSize(20).SemiBold();
                                txt.Line($"N°: {b.Id}".Trim());
                                txt.Line($"Fecha: {fecha}");
                                txt.Line($"Estado: {b.State}");
                            });

                            // Logo opcional:
                            // row.ConstantItem(120).Image("wwwroot/logo.png", ImageScaling.FitArea);
                        });
                    });

                    // ===== Content =====
                    page.Content().PaddingTop(10).Column(col =>
                    {
                        // Cliente
                        col.Item().Background(Colors.Grey.Lighten3).Padding(6).Text("Cliente").SemiBold();
                        col.Item().PaddingVertical(6).Text(txt =>
                        {
                            txt.Line($"Nombre: {b.Customer?.Nombre ?? "-"}");
                            txt.Line($"Teléfono: {b.Customer?.Telefono ?? "-"}");
                            txt.Line($"Email: {b.Customer?.Email ?? "-"}");
                            txt.Line($"Documento: {b.Customer?.TipoDocumento ?? "-"} {b.Customer?.Identificador ?? ""}".Trim());
                            var dir = b.Customer?.DireccionFiscal;
                            if (dir != null)
                                txt.Line($"Dirección: {dir.Calle} {dir.Numero} {dir.Ciudad} {dir.Departamento}".Replace("  ", " ").Trim());
                        });

                        // Detalle
                        col.Item().PaddingTop(10).Background(Colors.Grey.Lighten3).Padding(6).Text("Detalle").SemiBold();

                        if (productos.Count == 0)
                        {
                            col.Item().PaddingTop(6).Text("No hay productos en este presupuesto.")
                               .Italic().FontColor(Colors.Grey.Darken1);
                        }
                        else
                        {
                            int idx = 1;
                            foreach (var p in productos)
                            {
                                var nombre = p.Name ?? "Producto";
                                var tipo = p.ProductType.ToString();
                                var medidas = FormatearMedidasProducto(p);
                                var color = string.IsNullOrWhiteSpace(p.Color) ? "-" : p.Color;
                                var cant = p.Amount <= 0 ? 1 : p.Amount;
                                var unit = p.UnitPrice > 0 ? p.UnitPrice : (cant > 0 ? p.TotalPrice / cant : p.TotalPrice);

                                // Tarjeta del producto
                                col.Item().PaddingTop(8)
                                   .Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8)
                                   .Column(prod =>
                                   {
                                       prod.Item().Row(r =>
                                       {
                                           r.RelativeItem().Text(t =>
                                           {
                                               t.Span($"{idx}. ").SemiBold();
                                               t.Span(nombre).SemiBold();
                                               t.Span($"  ({tipo})").FontColor(Colors.Grey.Darken1);
                                           });

                                           r.ConstantItem(180).AlignRight().Text(t =>
                                           {
                                               t.Span("Subtotal: ").SemiBold();
                                               t.Span(FormatUyu(p.TotalPrice)).SemiBold(); // subtotal del producto en UYU (consistente con Total)
                                           });
                                       });

                                       prod.Item().Text($"Medidas: {medidas}   |   Color: {color}   |   Cant.: {cant}   |   Unit.: {FormatUyu(unit)}")
                                           .FontColor(Colors.Grey.Darken1);

                                       // ===== Tabla de insumos del producto =====
                                       var insumos = p.SuppliesUsed?.ToList() ?? new List<BudgetedSupply>();
                                       if (insumos.Count == 0)
                                       {
                                           prod.Item().PaddingTop(6).Text("Sin insumos desglosados.")
                                               .Italic().FontColor(Colors.Grey.Darken1);
                                       }
                                       else
                                       {
                                           prod.Item().PaddingTop(6).Text("Insumos").SemiBold();

                                           prod.Item().Table(table =>
                                           {
                                               table.ColumnsDefinition(c =>
                                               {
                                                   c.RelativeColumn(4);      // Insumo
                                                   c.RelativeColumn(2);      // Tipo
                                                   c.RelativeColumn(2.2f);   // Med. x cant. (string)
                                                   c.RelativeColumn(1.5f);   // Cant.
                                                   c.RelativeColumn(2);      // Unit. (U$S)
                                                   c.RelativeColumn(2);      // Subtotal (U$S)
                                                   c.RelativeColumn(4);      // Imagen (URL)
                                               });

                                               table.Header(h =>
                                               {
                                                   h.Cell().Element(CellHeader).Text("Insumo");
                                                   h.Cell().Element(CellHeader).Text("Tipo");
                                                   h.Cell().Element(CellHeader).Text("Med. x cant.");
                                                   h.Cell().Element(CellHeader).Text("Cant.");
                                                   h.Cell().Element(CellHeader).Text("Unit. (U$S)");
                                                   h.Cell().Element(CellHeader).Text("Subtotal (U$S)");
                                                   h.Cell().Element(CellHeader).Text("Imagen (URL)");
                                               });

                                               foreach (var s in insumos)
                                               {
                                                   var q = s.amount <= 0 ? 1 : s.amount;
                                                   // Si no tenés unitario, lo derivamos del subtotal
                                                   var unitario = q > 0 ? s.Subtotal / q : s.Subtotal;
                                                   var medidaXCant = FormatearMedidaXCantidad(s.UnitMeasure);

                                                   table.Cell().Element(Cell).Text(s.SupplyCode ?? "Insumo");
                                                   table.Cell().Element(Cell).Text(s.TypeSupply.ToString());
                                                   table.Cell().Element(Cell).AlignRight().Text(medidaXCant);
                                                   table.Cell().Element(Cell).AlignRight().Text(q.ToString(_uy));
                                                   table.Cell().Element(Cell).AlignRight().Text(FormatUsd(unitario)); // ← U$S
                                                   table.Cell().Element(Cell).AlignRight().Text(FormatUsd(s.Subtotal)); // ← U$S

                                                   var url = string.IsNullOrWhiteSpace(s.ImageUrl) ? "—" : s.ImageUrl.Trim();
                                                   table.Cell().Element(Cell).Text(t =>
                                                   {
                                                       if (EsUrlAbsoluta(url))
                                                       {
                                                           t.Hyperlink(url, url)
                                                            .FontColor(Colors.Blue.Medium)
                                                            .Underline();
                                                       }
                                                       else
                                                       {
                                                           t.Span(url);
                                                       }
                                                   });
                                               }

                                               static IContainer CellHeader(IContainer c) =>
                                                   c.DefaultTextStyle(x => x.SemiBold())
                                                    .Background(Colors.Grey.Lighten2)
                                                    .Padding(4);

                                               static IContainer Cell(IContainer c) =>
                                                   c.BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                                    .PaddingVertical(3).PaddingHorizontal(2);
                                           });
                                       }
                                   });

                                idx++;
                            }
                        }

                        // Total general (UYU)
                        col.Item().PaddingTop(12).AlignRight().Text(t =>
                        {
                            t.Span("TOTAL (UYU – pesos uruguayos): ").FontSize(12).SemiBold();
                            t.Span(FormatUyu(b.TotalPrice)).FontSize(12).SemiBold();
                        });

                        // Aclaración de monedas
                        col.Item().PaddingTop(6).Text("Notas: los importes Unit. y Subtotal de insumos se muestran en dólares estadounidenses (U$S). El total general está expresado en pesos uruguayos (UYU).")
                           .FontSize(9).FontColor(Colors.Grey.Darken2);
                    });

                    // ===== Footer =====
                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("ED Aluminios - ").Bold();
                        txt.Span("edaluminio@hotmail.com · +598 99 257 052");
                    });
                });
            }).GeneratePdf();

            return bytes;
        }

        // ===== Helpers =====

        private string FormatUsd(decimal value) => $"U$S {value.ToString("N2", _uy)}";
        private string FormatUyu(decimal value) => value.ToString("C", _uy);

        private string FormatearMedidasProducto(BudgetedProduct p)
        {
            // Si en tu modelo es Height (no Heigth), cambiá aquí.
            var ancho = p.Width > 0 ? $"{p.Width:0.##}" : "-";
            var alto = p.Heigth > 0 ? $"{p.Heigth:0.##}" : "-";
            return $"{ancho} x {alto}";
        }

        /// Normaliza el string de medida ("1,25 m", "2.5m²", etc.) y formatea el número.
        private string FormatearMedidaXCantidad(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "-";

            var s = raw.Trim();

            // Si parece expresión compuesta, devolver tal cual
            if (s.Contains("x", StringComparison.OrdinalIgnoreCase) || s.Contains("*") || s.Contains("/"))
                return s;

            var m = Regex.Match(s, @"^\s*([0-9]+(?:[.,][0-9]+)?)\s*(.*)\s*$");
            if (!m.Success) return s;

            var numPart = m.Groups[1].Value;
            var unitPart = m.Groups[2].Value?.Trim();

            if (!decimal.TryParse(numPart, NumberStyles.Number | NumberStyles.AllowDecimalPoint, _uy, out var val)
                && !decimal.TryParse(numPart, NumberStyles.Number | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out val))
                return s;

            var numFmt = Math.Round(val, 2, MidpointRounding.AwayFromZero).ToString("0.##", _uy);
            return string.IsNullOrWhiteSpace(unitPart) ? numFmt : $"{numFmt} {unitPart}";
        }

        private static bool EsUrlAbsoluta(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            return Uri.TryCreate(url, UriKind.Absolute, out var u)
                   && (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps);
        }
    }
}
