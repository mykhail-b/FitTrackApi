using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FitTrackApi.Infrastructure.Services;

public interface IEmailService
{
    Task SendEmail(string toEmail, string subject, string body, CancellationToken cancellationToken = default);
}

public record SmtpConfiguration
(
     string Host,
     int Port,
     bool UseSsl,
     string Username,
     string Password,
     string FromName,
     string FromEmail
);

public class EmailService : IEmailService
{
    private readonly SmtpConfiguration _settings;

    public EmailService(IOptions<SmtpConfiguration> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmail(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();

        var secureOption = _settings.UseSsl
            ? SecureSocketOptions.StartTls
            : SecureSocketOptions.None;

        await client.ConnectAsync(_settings.Host, _settings.Port, secureOption, cancellationToken);

        if (!string.IsNullOrWhiteSpace(_settings.Username))
        {
            await client.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
        }

        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}