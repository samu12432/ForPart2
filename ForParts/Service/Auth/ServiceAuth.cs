using ForParts.DTOs.Auth;
using ForParts.Exceptions.Auth;
using ForParts.IRepository.Auth;
using ForParts.IService.Auth;
using ForParts.JWT;
using ForParts.Models.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ForParts.Service.Auth
{
    public class ServiceAuth : IServiceAuth
    {
        private readonly Token _optionToken;
        private readonly IAuthRepository _userRepository;
        private readonly IServiceEmailAuth _emailSender;
        private readonly JwtSettingsConfirmation _jwt;

        public ServiceAuth(Token token, IAuthRepository repo, IServiceEmailAuth emailSender, IOptions<JwtSettingsConfirmation> jwtOptions)
        {
            _optionToken = token;
            _userRepository = repo;
            _emailSender = emailSender;
            _jwt = jwtOptions.Value;
        }

        //REGISTRO

        public async Task<bool> RegisterUser(RegisterDto dto)
        {

            //Validamos que el dto no sea vacio
            if (dto == null) throw new Exception("Datos incorrectos.");

            //Verificamos la existencia de el usuario a traves del Mail
            var exist = await _userRepository.GetByEmailAsync(dto.userEmail);
            if (exist != null)
                throw new EmailException("Ya existe un usuario creado con ese correo.");

            //Verificamos la existencia de el usuario a traves del UserName
            exist = await _userRepository.GetUserByInfo(dto.userName);
            if (exist != null)
                throw new UserNameException("Ya existe un usuario creado con ese nombre de usuario.");

            //Generamos un Hash con la contraseña
            CrearHashySalt(dto.password, out byte[] salt, out byte[] hash);

            //Generamos un usuario
            User newUser = new User(dto.name, dto.userEmail, dto.phoneNumber, dto.userName, hash, salt);

            //Accedemos a la Db y guardamos
            await _userRepository.AddAsync(newUser);

            //Se envia un mail de forma asicronica al progrma, para que el cliente verifique que existe realmente
            var token = GenerateEmailToken(newUser.userEmail);
            var link = $"https://localhost:7241/api/auth/confirm-email?token={token}";
            var body = $"Hola {newUser.userName}, haz clic aquí para confirmar tu correo: <a href='{link}'>Confirmar</a>";

            _ = Task.Run(() => _emailSender.SendAsync(
                newUser.userEmail,
                "Confirmación de correo",
                $"Hola {newUser.userName}, haz clic aquí para confirmar tu correo: <a href='{link}'>Confirmar</a>"));
            return true;
        }

        private void CrearHashySalt(string password, out byte[] salt, out byte[] hash)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }


        private string GenerateEmailToken(string email)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Email, email),
            new Claim("purpose", "email_confirmation")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //LOGIN 
        public async Task<(string? token, User user)> LoginUser(LoginDto dto)
        {
            //Validamos que el dto no sea vacio
            if (dto == null) throw new Exception("Datos incorrectos.");

            //Obtenemos el usuario
            var user = await _userRepository.GetUserByInfo(dto.userName);

            //Validamos que no exista un usuario y verificamos password (a traves del hash y salt, DEBEN SER LAS MISMAS)
            if (user == null || !VerificarPassword(dto.password, user.passwordHash, user.passwordSalt)) throw new Exception("Lo sentimos, credenciales incorrectas.");

            //Verificar si el usuario ha confirmado su correo electrónico
            var confirmed = await _userRepository.UserByEmailConfirmed(user.userEmail);
            if (!confirmed)
                throw new EmailException("Por favor, confirme su correo electrónico antes de iniciar sesión.");

            //Generamos un token y devolvemos para iniciar la sesion
            var t = _optionToken.GenerateToken(user);
            return (t, user);
        }


        private bool VerificarPassword(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return hash.SequenceEqual(computedHash);
        }


        //VERIFICACION DE MAIL

        public async Task<bool> ConfirmEmailAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwt.Issuer,
                    ValidAudience = _jwt.Audience,
                    IssuerSigningKey = key,
                    ValidateLifetime = true
                }, out _);

                var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                if (email == null) return false;

                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null) return false;

                user.IsEmailConfirmed = true;
                await _userRepository.UpdateAsync(user);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
