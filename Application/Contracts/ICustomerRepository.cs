using NhjDotnetApi.Domain.Entities;

namespace NhjDotnetApi.Application.Contracts;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(Guid id);

    Task<bool> ExistsByUserIdAsync(Guid userId);
    Task AddAsync(Customer customer);

    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Customer customer);
}

