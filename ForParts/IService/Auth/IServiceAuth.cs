using ForParts.DTOs.Auth;

namespace ForParts.IService.Auth
{
    public interface IServiceAuth
    {
        Task<bool> RegisterUser(RegisterDto dto);

        Task<bool> ConfirmEmailAsync(string token);

        Task<string?> LoginUser(LoginDto dto);
    }
}
