using ForParts.Data;
using ForParts.IRepository.Auth;
using ForParts.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace ForParts.Repository.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ContextDb _context;

        public AuthRepository(ContextDb context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); //En el caso de error con la conexion en la Bd
        }

        public async Task AddAsync(User user) //Se guarda en BD
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email) //Se realiza busqueda utilizando el INDEX creado
        {
            return await _context.Users.Where(u => u.userEmail == email).FirstOrDefaultAsync(); 
        }

        public async Task<bool> UserByEmailConfirmed(string mail)
        {
            return await _context.Users
                .Where(u => u.userEmail == mail && u.IsEmailConfirmed)
                .AnyAsync();
        }

        public async Task<User?> GetUserByInfo(string nameUser)
        {
            return await _context.Users.Where(u => u.userName == nameUser).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
