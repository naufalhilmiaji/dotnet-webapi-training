using System.Linq;
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

    public OrderService(AppDbContext db, IOrderNotificationService notifier)
    {
        _db = db;
        _notifier = notifier;
    }

    public Order CreateOrder(CreateOrderRequest request, Guid userId)
    {
        var productIds = request.Items.Select(i => i.ProductId).ToList();

        var customer = _db.Customers.Find(request.CustomerId);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        var products = _db.Products.Where(p => productIds.Contains(p.Id)).ToDictionary(p => p.Id);

        // ✅ VALIDATE STOCK
        foreach (var item in request.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
                throw new InvalidOperationException("Product not found");

            if (product.Stock < item.Quantity)
                throw new InvalidOperationException(
                    $"Insufficient stock for product {product.Name}"
                );
        }

        // 🔥 DEDUCT STOCK
        foreach (var item in request.Items)
        {
            products[item.ProductId].Stock -= item.Quantity;
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CustomerId = request.CustomerId,
            CustomerName = customer.Name,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Items = request
                .Items.Select(i =>
                {
                    var product = products[i.ProductId];
                    return new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        ProductName = product.Name,
                        Quantity = i.Quantity,
                        Price = product.Price,
                    };
                })
                .ToList(),
        };

        order.TotalAmount = order.Items.Sum(i => i.Price * i.Quantity);

        _db.Orders.Add(order);
        _db.SaveChanges();

        return order;
    }

    public List<Order> GetAll(Guid userId, bool isAdmin)
    {
        if (isAdmin)
            return _db.Orders.Include(o => o.Items).ToList();

        var orders = _db
            .Orders.Include(o => o.Items)
            .Include(o => o.Customer)
            .Where(o => o.UserId == userId)
            .ToList();

        foreach (var order in orders)
        {
            if (string.IsNullOrEmpty(order.CustomerName) && order.Customer != null)
            {
                order.CustomerName = order.Customer.Name;
            }
        }

        return orders;
    }

    public Order? GetById(Guid id, Guid userId, bool isAdmin)
    {
        if (isAdmin)
            return _db.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);

        var order = _db
            .Orders.Include(o => o.Items)
            .Include(o => o.Customer)
            .FirstOrDefault(o => o.Id == id && o.UserId == userId);

        if (order != null && string.IsNullOrEmpty(order.CustomerName) && order.Customer != null)
            order.CustomerName = order.Customer.Name;

        return order;
    }

    public async Task<bool> UpdateStatusAsync(
        Guid orderId,
        OrderStatus newStatus,
        Guid currentUserId,
        bool isAdmin
    )
    {
        var order = _db.Orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
            return false;

        // USER hanya boleh update order miliknya
        if (!isAdmin && order.UserId != currentUserId)
            return false;

        order.Status = newStatus;
        await _db.SaveChangesAsync();

        // 🔔 Kirim real-time notification
        await _notifier.NotifyStatusChangedAsync(order.Id, order.UserId, order.Status.ToString());

        return true;
    }

    public bool CancelOrder(Guid id, Guid currentUserId, bool isAdmin)
    {
        var order = _db.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
            return false;

        // USER hanya boleh cancel order sendiri
        if (!isAdmin && order.UserId != currentUserId)
            return false;

        order.Status = OrderStatus.Cancelled;
        _db.SaveChanges();
        return true;
    }
}
