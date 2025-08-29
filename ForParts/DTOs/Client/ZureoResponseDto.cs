namespace ForParts.DTOs.Client
{
    public class ZureoResponseDto
    {
        /// <summary>
        /// Indica si la emisión fue exitosa (HTTP 200).
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo del resultado (éxito o error).
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Cuerpo crudo de la respuesta HTTP (JSON, XML, etc).
        /// </summary>
        public string Raw { get; set; } = string.Empty;

        /// <summary>
        /// Código HTTP devuelto por Zureo (200, 400, 500, etc).
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// ID externo de la factura en Zureo (si fue emitida).
        /// </summary>
        public string? ExternalInvoiceId { get; set; }

        /// <summary>
        /// Detalles de error si la emisión falló.
        /// </summary>
        public Dictionary<string, string>? ErrorDetails { get; set; }
    }

}
