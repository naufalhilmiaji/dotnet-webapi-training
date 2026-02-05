namespace NhjDotnetApi.Application.Contracts;

public interface IEmailService
{
    Task SendAsync(
        string toEmail,
        string subject,
        string body
    );
}
