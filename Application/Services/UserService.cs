using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Domain.Entities;
using NhjDotnetApi.Persistence;
using NhjDotnetApi.Presentation.Models;

namespace NhjDotnetApi.Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public void Register(RegisterUserRequest request)
    {
        if (_db.Users.Any(u => u.Username == request.Username))
            throw new InvalidOperationException("Username already exists");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            DisplayName = request.DisplayName,
            Role = "USER"
        };

        _db.Users.Add(user);
        _db.SaveChanges();
    }
}
