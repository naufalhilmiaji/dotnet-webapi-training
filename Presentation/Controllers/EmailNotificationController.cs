using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Presentation.Models;

namespace NhjDotnetApi.Presentation.Controllers;

[ApiController]
[Route("api/notification")]
[Authorize(Roles = "ADMIN")]
public class EmailNotificationController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailNotificationController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("email")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.To) ||
            string.IsNullOrWhiteSpace(request.Subject) ||
            string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("To, Subject, and Body are required.");
        }

        await _emailService.SendAsync(
            request.To,
            request.Subject,
            request.Body
        );

        return Ok(new
        {
            success = true,
            message = "Email sent successfully."
        });
    }
}
