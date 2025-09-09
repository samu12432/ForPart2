using AutoMapper;
using System.Net.Http.Headers;
using System.Text;
using ForParts.DTOs.FEU;
using ForParts.Exceptions.Invoice;
using ForParts.IRepository.Invoice;
using ForParts.IService.FEU;
using ForParts.Models.Enums;
using Newtonsoft.Json;

namespace ForParts.Service.FEU
{
    public class FeuService : IFeuService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public FeuService(IHttpClientFactory httpClientFactory, IInvoiceRepository invoiceRepository, IConfiguration config, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _invoiceRepository = invoiceRepository;
            _config = config;
            _mapper = mapper;
        }

        public async Task<FeuResponseDto> EmitirFacturaAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdWithItemsAsync(id);
            if (invoice == null)
                throw new InvoiceException("Factura no encontrada.");

            ValidarFactura(invoice);

            var dto = _mapper.Map<FeuInvoiceDto>(invoice);
            var json = JsonConvert.SerializeObject(dto);

            var token = await ObtenerTokenAsync();

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync("https://api-test.facturaelectronica.com.uy/comprobantes/crear", new StringContent(json, Encoding.UTF8, "application/json"));
            var raw = await response.Content.ReadAsStringAsync();

            invoice.InvoiceState = InvoiceState.Emitir;
            invoice.FechaEmision = DateTime.Now;
            invoice.ZureoRespuesta = $"Emitida en FEU: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            await _invoiceRepository.UpdateAsync(invoice);

            return new FeuResponseDto
            {
                Success = response.IsSuccessStatusCode,
                Raw = raw,
                Message = response.IsSuccessStatusCode ? "Factura emitida correctamente en FEU." : "Error al emitir factura en FEU."
            };
        }

        private async Task<string> ObtenerTokenAsync()
        {
            var username = _config["FeuApi:Username"];
            var password = _config["FeuApi:Password"];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new InvoiceException("Credenciales de FEU no configuradas correctamente.");

            var client = _httpClientFactory.CreateClient();
            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("grant_type", "password"),
        new KeyValuePair<string, string>("username", username),
        new KeyValuePair<string, string>("password", password)
    });

            var response = await client.PostAsync("https://auth-test.facturaelectronica.com.uy/token", content);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new InvoiceException("Error al obtener token de FEU: " + json);

            var tokenObj = JsonConvert.DeserializeObject<FeuTokenDto>(json);
            return tokenObj.access_token;
        }


        private void ValidarFactura(Models.Invoice.Invoice invoice)
        {
            if (invoice.Customer == null)
                throw new InvoiceException("La factura no tiene cliente asignado.");

            if (invoice.Items == null || !invoice.Items.Any())
                throw new InvoiceException("La factura no contiene ítems.");

            foreach (var item in invoice.Items)
            {
                if (item.Product == null)
                    throw new InvoiceException($"Producto '{item.ProductCode}' no vinculado.");
                if (item.Product.productPrice <= 0)
                    throw new InvoiceException($"Producto '{item.ProductCode}' con precio inválido.");
            }
        }
    }
}
