namespace ForParts.IService.Auth
{
    public interface IServiceEmailAuth
    {
        Task SendAsync(string to, string subject, string body);

    }
}
