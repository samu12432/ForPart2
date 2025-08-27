namespace ForParts.IServices.Image
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile image, string folder);

    }
}
