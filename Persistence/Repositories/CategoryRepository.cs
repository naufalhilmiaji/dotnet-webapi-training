using Microsoft.EntityFrameworkCore;
using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Domain.Entities;

namespace NhjDotnetApi.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _db;

    public CategoryRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Category>> GetAllAsync()
        => await _db.Categories.ToListAsync();

    public async Task<Category?> GetByIdAsync(Guid id)
        => await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(Category category)
    {
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _db.Categories.Update(category);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
    }
}
