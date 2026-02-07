using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Application.DTOs;
using NhjDotnetApi.Domain.Entities;

namespace NhjDotnetApi.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repo;

    public CustomerService(ICustomerRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<CustomerResponse>> GetAllAsync()
    {
        var customers = await _repo.GetAllAsync();

        return customers.Select(c => new CustomerResponse
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone
        }).ToList();
    }

    public async Task<CustomerResponse?> GetByIdAsync(Guid id)
    {
        var customer = await _repo.GetByIdAsync(id);
        if (customer == null) return null;

        return new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone
        };
    }

    public async Task CreateAsync(CustomerRequest request, Guid userId)
    {
        // (opsional tapi bagus) 1 user = 1 customer
        var exists = await _repo.ExistsByUserIdAsync(userId);
        if (exists)
            throw new InvalidOperationException(
                "User already has a customer"
            );

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(customer);
    }

    public async Task<bool> UpdateAsync(Guid id, CustomerRequest request)
    {
        var customer = await _repo.GetByIdAsync(id);
        if (customer == null) return false;

        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.Phone = request.Phone;

        await _repo.UpdateAsync(customer);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await _repo.GetByIdAsync(id);
        if (customer == null) return false;

        await _repo.DeleteAsync(customer);
        return true;
    }
}
