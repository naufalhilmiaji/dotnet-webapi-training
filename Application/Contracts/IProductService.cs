using NhjDotnetApi.Application.DTOs;

namespace NhjDotnetApi.Application.Contracts;

public interface IProductService
{
    Task<List<ProductResponse>> GetAllAsync();
    Task<ProductResponse?> GetByIdAsync(Guid id);
    Task CreateAsync(ProductRequest request);
    Task<bool> UpdateAsync(Guid id, ProductRequest request);
    Task<bool> DeleteAsync(Guid id);
}
