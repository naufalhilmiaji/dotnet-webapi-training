using NhjDotnetApi.Application.DTOs;

namespace NhjDotnetApi.Application.Contracts;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllAsync();
    Task<CategoryResponse?> GetByIdAsync(Guid id);
    Task CreateAsync(CategoryRequest request);
    Task<bool> UpdateAsync(Guid id, CategoryRequest request);
    Task<bool> DeleteAsync(Guid id);
}
