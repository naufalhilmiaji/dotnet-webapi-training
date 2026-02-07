using Microsoft.AspNetCore.Mvc;
using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Presentation.Models;

namespace NhjDotnetApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponseDto> Login(LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new LoginResponseDto
            {
                Success = false,
                Message = "Username and password are required."
            });
        }

        var result = _authService.Login(request);

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }
}
