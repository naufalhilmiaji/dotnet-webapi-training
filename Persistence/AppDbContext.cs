using Microsoft.EntityFrameworkCore;
using NhjDotnetApi.Domain.Entities;

namespace NhjDotnetApi.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.CustomerName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(o => o.TotalAmount)
                  .HasPrecision(18, 2);

            entity.HasMany(o => o.Items)
                  .WithOne()
                  .HasForeignKey(i => i.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(o => o.User)
                  .WithMany()
                  .HasForeignKey(o => o.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.ProductName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(i => i.Price)
                  .HasPrecision(18, 2);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(u => u.PasswordHash)
                  .IsRequired();

            entity.Property(u => u.DisplayName)
                  .HasMaxLength(100);

            entity.Property(u => u.Role)
                  .IsRequired()
                  .HasMaxLength(20);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(o => o.User)
                  .WithMany()
                  .HasForeignKey(o => o.UserId);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Price)
                  .HasPrecision(18, 2);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.Name)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(c => c.Email)
                  .HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(o => o.Customer)
                  .WithMany(c => c.Orders)
                  .HasForeignKey(o => o.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });


    }

}
