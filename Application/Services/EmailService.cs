using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Persistence.Configurations;

namespace NhjDotnetApi.Application.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(
            _settings.FromName,
            _settings.FromAddress
        ));

        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _settings.Host,
            _settings.Port,
            SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(
            _settings.Username,
            _settings.Password
        );

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
