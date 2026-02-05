using NhjDotnetApi.Application.DTOs;

namespace NhjDotnetApi.Application.Contracts;

public interface ICustomerService
{
    Task<List<CustomerResponse>> GetAllAsync();
    Task<CustomerResponse?> GetByIdAsync(Guid id);
    Task CreateAsync(CustomerRequest request);
    Task<bool> UpdateAsync(Guid id, CustomerRequest request);
    Task<bool> DeleteAsync(Guid id);
}
