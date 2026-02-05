using Microsoft.EntityFrameworkCore;

using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Domain.Entities;
using NhjDotnetApi.Persistence;
using NhjDotnetApi.Presentation.Models;

namespace NhjDotnetApi.Application.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;

    private readonly IOrderNotificationService _notifier;
    public OrderService(
        AppDbContext db,
        IOrderNotificationService notifier)
    {
        _db = db;
        _notifier = notifier;
    }


    public Order CreateOrder(CreateOrderRequest request, Guid userId)
    {
        var customer = _db.Customers
            .FirstOrDefault(c => c.Id == request.CustomerId);

        if (customer == null)
        {
            throw new InvalidOperationException(
                $"Customer with ID {request.CustomerId} not found"
            );
        }


        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CustomerName = customer.Name,
            UserId = userId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Items = request.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };


        order.TotalAmount = order.Items.Sum(i => i.Quantity * i.Price);

        _db.Orders.Add(order);
        _db.SaveChanges();

        return order;
    }

    public List<Order> GetAll(Guid userId, bool isAdmin)
    {
        if (isAdmin)
            return _db.Orders.Include(o => o.Items).ToList();

        return _db.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .ToList();
    }

    public Order? GetById(Guid id, Guid userId, bool isAdmin)
    {
        if (isAdmin)
            return _db.Orders.Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);

        return _db.Orders.Include(o => o.Items)
            .FirstOrDefault(o => o.Id == id && o.UserId == userId);
    }

    public async Task<bool> UpdateStatusAsync(
        Guid orderId,
        OrderStatus newStatus,
        Guid currentUserId,
        bool isAdmin)
    {
        var order = _db.Orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null) return false;

        // USER hanya boleh update order miliknya
        if (!isAdmin && order.UserId != currentUserId)
            return false;

        order.Status = newStatus;
        await _db.SaveChangesAsync();

        // 🔔 Kirim real-time notification
        await _notifier.NotifyStatusChangedAsync(
            order.Id,
            order.UserId,
            order.Status.ToString()
        );

        return true;
    }

    public bool CancelOrder(Guid id, Guid currentUserId, bool isAdmin)
    {
        var order = _db.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return false;

        // USER hanya boleh cancel order sendiri
        if (!isAdmin && order.UserId != currentUserId)
            return false;

        order.Status = OrderStatus.Cancelled;
        _db.SaveChanges();
        return true;
    }


}

