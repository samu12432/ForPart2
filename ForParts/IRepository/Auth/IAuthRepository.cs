using ForParts.Models.Auth;

namespace ForParts.IRepository.Auth
{
    public interface IAuthRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetUserByInfo(string nameUser);
        Task AddAsync(User user);
        Task<bool> UserByEmailConfirmed(string mail);
        Task UpdateAsync(User user);
    }
}
