namespace ForParts.DTOs.FEU
{
    public class FeuResponseDto
    {
        public bool Success { get; set; }
        public string Raw { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
