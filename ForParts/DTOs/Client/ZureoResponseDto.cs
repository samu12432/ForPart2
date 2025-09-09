namespace ForParts.DTOs.Client
{
    public class ZureoResponseDto
    {
        public bool Success { get; set; }
        public string Archivo { get; set; } = string.Empty;
        public string Ruta { get; set; } = string.Empty;
        public string Raw { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        /*public bool Success { get; set; }
          public string Message { get; set; } = string.Empty;
          public string Raw { get; set; } = string.Empty;
          public int StatusCode { get; set; }
          public string? ExternalInvoiceId { get; set; }
          public Dictionary<string, string>? ErrorDetails { get; set; }*/
    }

}
