using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Application.DTOs;
using NhjDotnetApi.Domain.Entities;

namespace NhjDotnetApi.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        var products = await _repo.GetAllAsync();
        return products.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price
        }).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product == null) return null;

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };
    }

    public async Task CreateAsync(ProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };

        await _repo.AddAsync(product);
    }

    public async Task<bool> UpdateAsync(Guid id, ProductRequest request)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product == null) return false;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;

        await _repo.UpdateAsync(product);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product == null) return false;

        await _repo.DeleteAsync(product);
        return true;
    }
}
