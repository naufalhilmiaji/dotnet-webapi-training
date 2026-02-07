using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Domain.Entities;
using NhjDotnetApi.Presentation.Models;
using NhjDotnetApi.Persistence;

namespace NhjDotnetApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public LoginResponseDto Login(LoginRequestDto request)
    {
        var user = _db.Users
            .SingleOrDefault(u =>
                u.Username.ToLower() == request.Username.ToLower());

        if (user == null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid username or password."
            };
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash
        );

        if (!isPasswordValid)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid username or password."
            };
        }

        var jwt = _config.GetSection("Jwt");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        );

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                Convert.ToDouble(jwt["ExpireMinutes"])
            ),
            signingCredentials: new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256)
        );

        return new LoginResponseDto
        {
            Success = true,
            Message = "Login successful.",
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            DisplayName = user.DisplayName,
            Role = user.Role
        };
    }

}
