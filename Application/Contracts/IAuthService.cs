using NhjDotnetApi.Presentation.Models;

namespace NhjDotnetApi.Application.Contracts;

public interface IAuthService
{
    LoginResponseDto Login(LoginRequestDto request);
}
