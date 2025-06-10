using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Proyecto.Model.ViewModel;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Proyecto.Services
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;
        public EmailService(SmtpSettings smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public async Task SendEmailAsync(string destinatario, string asunto, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(destinatario));
            message.Subject = asunto;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();

            // Selecciona la opción de seguridad correcta según el puerto
            SecureSocketOptions sslOptions;
            if (_smtpSettings.Port == 465)
                sslOptions = SecureSocketOptions.SslOnConnect;
            else if (_smtpSettings.Port == 587)
                sslOptions = SecureSocketOptions.StartTls;
            else
                sslOptions = SecureSocketOptions.Auto;

            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, sslOptions);

            if (!string.IsNullOrWhiteSpace(_smtpSettings.Username))
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}