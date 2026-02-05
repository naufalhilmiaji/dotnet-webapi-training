using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Application.DTOs;
using NhjDotnetApi.Domain.Entities;

namespace NhjDotnetApi.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<CategoryResponse>> GetAllAsync()
    {
        var categories = await _repo.GetAllAsync();
        return categories.Select(c => new CategoryResponse
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        }).ToList();
    }

    public async Task<CategoryResponse?> GetByIdAsync(Guid id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null) return null;

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task CreateAsync(CategoryRequest request)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        await _repo.AddAsync(category);
    }

    public async Task<bool> UpdateAsync(Guid id, CategoryRequest request)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null) return false;

        category.Name = request.Name;
        category.Description = request.Description;

        await _repo.UpdateAsync(category);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null) return false;

        await _repo.DeleteAsync(category);
        return true;
    }
}
