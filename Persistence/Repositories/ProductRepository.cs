using Microsoft.EntityFrameworkCore;
using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Domain.Entities;

namespace NhjDotnetApi.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> GetAllAsync()
        => await _db.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(Guid id)
        => await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
    }
}
