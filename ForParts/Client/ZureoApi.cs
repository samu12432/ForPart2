using ForParts.Exceptions.ClientZureo;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using ForParts.DTOs.Invoice;

namespace ForParts.Client
{
    public class ZureoApi
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ZureoApi> _logger;

        private const string BaseUrl = "https://api.zureo.com";
        private const string TokenCacheKey = "ZureoAuthToken";

        public ZureoApi(HttpClient httpClient, IConfiguration configuration, IMemoryCache cache, ILogger<ZureoApi> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> AuthenticateAsync()  //Necesitamos loguearnos a Zureo con el usuario de nuestro cliente
        {
            if (_cache.TryGetValue(TokenCacheKey, out string? token))
            {
                _logger.LogInformation("Token JWT recuperado desde caché.");
                return token;
            }

            _logger.LogInformation("Autenticando con Zureo...");

            var credentials = _configuration["Zureo:Credentials"];
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/sdk/v1/security/login");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encoded);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error autenticando con Zureo: {Status} {Body}", response.StatusCode, errorBody);
                throw new ZureoClientAuthException("Error al autenticar con Zureo", errorBody);
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            token = doc.RootElement.GetProperty("token").GetString();

            _cache.Set(TokenCacheKey, token, TimeSpan.FromHours(1));

            _logger.LogInformation("Token JWT almacenado en caché.");
            return token;
        }
        /*
        public async Task<string> SendInvoiceAsync(InvoiceDto dto)
        {
             _logger.LogInformation("Enviando factura a Zureo para la empresa {Empresa}", dto.ToString);

             var token = await AuthenticateAsync();
             var payload = BuildPayload(dto);
             var json = JsonSerializer.Serialize(payload);
             var content = new StringContent(json, Encoding.UTF8, "application/json");

             var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/sdk/v1/order/add");
             request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
             request.Content = content;

             var response = await _httpClient.SendAsync(request);
             if (!response.IsSuccessStatusCode)
             {
                 var errorBody = await response.Content.ReadAsStringAsync();
                 _logger.LogError("Error enviando factura: {Status} {Body}", response.StatusCode, errorBody);
                 throw new ZureoClientApiException("Error al enviar la factura", errorBody);
             }

             var result = await response.Content.ReadAsStringAsync();
             _logger.LogInformation("Factura enviada correctamente: {Response}", result);
            return "result";
        }

       private static object BuildPayload(InvoiceDto dto) => new
       {
            Fecha = dto.Fecha,
            Empresa = dto.Empresa,
            Sucursal = new { Id = dto.SucursalId },
            Tipo = new { Nombre = dto.TipoNombre },
            Moneda = new { CurrencyCode = dto.Moneda },
            Contacto = new
            {
                TipoEnte = 1,
                Identificador1 = new { Value = dto.Contacto.Identificador },
                Nombre1 = new { Value = dto.Contacto.Nombre }
            },
            Articulos = dto.Articulos.Select(a => new
            {
                Articulo = new { Codigo = a.Codigo, Nombre = a.Nombre },
                Cantidad = a.Cantidad,
                PrecioUnitario = a.PrecioUnitario
            }),
            ImporteTotal = dto.ImporteTotal,
            TipoCambio = dto.TipoCambio,
            Comentario = dto.Comentario,
            Financiacion = new
            {
                ProximoVencimiento = DateTime.UtcNow.AddDays(30).ToString("yyyy-MM-dd")
            }
        };*/
    }
}
//Se hace uso de la Api de Zure para poder emitir facturas electronicas