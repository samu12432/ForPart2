using ForParts.DTOs.Auth;
using ForParts.Models.Auth;

namespace ForParts.IService.Auth
{
    public interface IServiceAuth
    {
        Task<bool> RegisterUser(RegisterDto dto);

        Task<bool> ConfirmEmailAsync(string token);

        Task<(string? token, User user)> LoginUser(LoginDto dto);
    }
}
