using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Presentation.Models;

namespace NhjDotnetApi.Presentation.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public IActionResult Register(RegisterUserRequest request)
    {
        _userService.Register(request);

        return Ok(new
        {
            message = "User registered successfully"
        });
    }
}
