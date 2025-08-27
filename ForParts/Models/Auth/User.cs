using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ForParts.Models.Auth
{
    [Table("Users")]
    [Index(nameof(userEmail), IsUnique = true)]
    [Index(nameof(userName), IsUnique = true)]
    [Index(nameof(phoneNumber), IsUnique = true)]

    public class User
    {
        [Key]
        public int id { get; set; }

        //Datos personales
        [Required]
        public string name { get; set; } = string.Empty;
        [Required]
        public string userEmail { get; set; } = string.Empty;
        [Required]
        public string phoneNumber { get; set; } = string.Empty;

        [Required]
        public string userName { get; set; } = string.Empty; // Nombre de usuario para login

        //Datos para usario de uso
        [Required]
        public byte[] passwordHash { get; set; }
        [Required]
        public byte[] passwordSalt { get; set; }


        public bool IsEmailConfirmed { get; set; } = false; // Para confirmar que el mail existe, luego de realizar un registro. Sin ello, el usuario no puede utilizar la aplicación.

        public User() { }
        public User(string name, string userEmail, string phoneNumber, string userName, byte[] passwordHash, byte[] passwordSalt)
        {
            this.name = name;
            this.userEmail = userEmail;
            this.userName = userName;
            this.phoneNumber = phoneNumber;
            this.passwordHash = passwordHash;
            this.passwordSalt = passwordSalt;
        }
    }
}
