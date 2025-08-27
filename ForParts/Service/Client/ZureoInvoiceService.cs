using AutoMapper;
using ForParts.DTOs.Invoice;
using ForParts.IService.Client;
using InvoiceAlias = ForParts.Models.Invoice.Invoice;
using Newtonsoft.Json;
using System.Text;

namespace ForParts.Service.Client
{
    public class ZureoInvoiceService : IZureoInvoiceService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public ZureoInvoiceService(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        public async Task<ZureoResponseDto> EmitirAsync(InvoiceAlias invoice)
        {
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
        }
    }
}
