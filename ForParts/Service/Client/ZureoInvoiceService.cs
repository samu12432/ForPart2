using AutoMapper;
using ForParts.DTOs.Invoice;
using ForParts.IService.Client;
using InvoiceAlias = ForParts.Models.Invoice.Invoice;
using Newtonsoft.Json;
using System.Text;
using ForParts.DTOs.Client;
using ForParts.Exceptions.Invoice;
using ForParts.IRepository.Invoice;
using ForParts.Models.Enums;

namespace ForParts.Service.Client
{
    public class ZureoInvoiceService : IZureoInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly string _zureoPath;

        public ZureoInvoiceService(IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _zureoPath = Path.Combine(desktopPath, "ZureoPendientes");
            Directory.CreateDirectory(_zureoPath);
        }

        public async Task<ZureoResponseDto?> EmitirFacturaAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdWithItemsAsync(id);
            if (invoice == null)
                throw new InvoiceException("Factura no encontrada.");

            ValidarFactura(invoice);

            var dto = _mapper.Map<ZureoInvoiceDto>(invoice);
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);

            var fileName = $"FAC_{id}_{DateTime.Now:yyyyMMddHHmmss}.json";
            var fullPath = Path.Combine(_zureoPath, fileName);

            await File.WriteAllTextAsync(fullPath, json, Encoding.UTF8);

            invoice.InvoiceState = InvoiceState.Emitir;
            invoice.FechaEmision = DateTime.Now;
            invoice.ZureoRespuesta = $"Archivo generado: {fileName}";
            await _invoiceRepository.UpdateAsync(invoice);

            return new ZureoResponseDto
            {
                Success = true,
                Archivo = fileName,
                Ruta = fullPath,
                Raw = json,
                Message = "Factura emitida correctamente y lista para ser procesada por Zureo."
            };
        }

        private void ValidarFactura(InvoiceAlias invoice)
        {
            if (invoice.Customer == null)
                throw new InvoiceException("La factura no tiene cliente asignado.");

            if (invoice.Items == null || !invoice.Items.Any())
                throw new InvoiceException("La factura no contiene ítems.");

            foreach (var item in invoice.Items)
            {
                if (item.Product == null)
                    throw new InvoiceException($"El producto '{item.ProductCode}' no está vinculado correctamente.");
                if (item.Product.productPrice <= 0)
                    throw new InvoiceException($"El producto '{item.ProductCode}' tiene un precio inválido.");
            }
        }
    }

    /*private readonly IHttpClientFactory _httpClientFactory;
    private readonly IInvoiceRepository invoiceRepository;
    private readonly IMapper _mapper;

    public ZureoInvoiceService(IHttpClientFactory httpClientFactory, IMapper mapper, IInvoiceRepository invoiceRepository)
    {
        _httpClientFactory = httpClientFactory;
        _mapper = mapper;
        this.invoiceRepository = invoiceRepository;
    }

    public async Task<ZureoResponseDto?> EmitirFacturaAsync(int id)
    {
        var invoice = await invoiceRepository.GetByIdAsync(id);
        var dto = _mapper.Map<ZureoInvoiceDto>(invoice);
        var json = JsonConvert.SerializeObject(dto);

        var client = _httpClientFactory.CreateClient("Zureo");
        var response = await client.PostAsync("/api/facturas", new StringContent(json, Encoding.UTF8, "application/json"));
        var raw = await response.Content.ReadAsStringAsync();

        return new ZureoResponseDto
        {
            Success = response.IsSuccessStatusCode,
            Raw = raw,
            Message = response.IsSuccessStatusCode ? "Factura emitida correctamente." : "Error al emitir factura."
        };
    }*/
}
