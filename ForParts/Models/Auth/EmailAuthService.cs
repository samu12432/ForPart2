using System.Net.Mail;
using System.Net;
using ForParts.IService.Auth;

namespace ForParts.Services.Auth
{
    public class EmailAuthService : IServiceEmailAuth
    {
        private readonly IConfiguration _config;

        public EmailAuthService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string toEmail, string subject, string body)
        {
            string? host = _config["Smtp:Host"];
            string? portString = _config["Smtp:Port"];
            string? user = _config["Smtp:User"];
            string? password = _config["Smtp:Password"];
            string? sender = _config["Smtp:Sender"];

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(portString)
                || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(sender))
            {
                throw new InvalidOperationException("La configuración SMTP no está completa.");
            }

            if (!int.TryParse(portString, out int port))
            {
                throw new InvalidOperationException("El puerto SMTP no es un número válido.");
            }

            var smtp = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(user, password),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(sender),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);
            await smtp.SendMailAsync(mail);
        }
    }
}
