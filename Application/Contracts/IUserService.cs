using NhjDotnetApi.Presentation.Models;

namespace NhjDotnetApi.Application.Contracts;

public interface IUserService
{
    void Register(RegisterUserRequest request);
}
