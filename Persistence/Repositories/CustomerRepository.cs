using Microsoft.EntityFrameworkCore;
using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Domain.Entities;

namespace NhjDotnetApi.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Customer>> GetAllAsync()
        => await _db.Customers.ToListAsync();

    public async Task<Customer?> GetByIdAsync(Guid id)
        => await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(Customer customer)
    {
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _db.Customers.Update(customer);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Customer customer)
    {
        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync();
    }
}
