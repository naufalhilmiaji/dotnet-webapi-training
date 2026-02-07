using NhjDotnetApi.Domain.Entities;
using BCrypt.Net;

namespace NhjDotnetApi.Persistence;

public static class DataSeeder
{
    public static void SeedAdmin(AppDbContext db)
    {
        if (db.Users.Any(u => u.Role == "ADMIN"))
            return;

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            DisplayName = "System Administrator",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role = "ADMIN"
        };

        db.Users.Add(admin);
        db.SaveChanges();
    }
}
