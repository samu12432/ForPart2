namespace ForParts.IDto
{
    public interface IImageDto
    {
        public IFormFile? Image { get; set; }

        public string? imageUrl { get; set; }
    }
} 
