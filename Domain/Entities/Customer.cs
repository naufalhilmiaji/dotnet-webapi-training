namespace NhjDotnetApi.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }     // 🔥 HARUS ADA
    public User User { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

