using Microsoft.EntityFrameworkCore;
using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Application.DTOs;
using NhjDotnetApi.Domain.Entities;
using NhjDotnetApi.Persistence;

namespace NhjDotnetApi.Application.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        return await _db
            .Products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
            })
            .ToListAsync();
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return null;

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
        };
    }

    public async Task CreateAsync(ProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Guid id, ProductRequest request)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return false;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return false;

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }
}
