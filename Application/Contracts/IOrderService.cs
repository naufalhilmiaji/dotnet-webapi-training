using NhjDotnetApi.Domain.Entities;
using NhjDotnetApi.Presentation.Models;

namespace NhjDotnetApi.Application.Contracts;

public interface IOrderService
{
    Order CreateOrder(CreateOrderRequest request, Guid userId);

    List<Order> GetAll(Guid userId, bool isAdmin);

    Order? GetById(Guid id, Guid userId, bool isAdmin);

    Task<bool> UpdateStatusAsync(
        Guid orderId,
        OrderStatus newStatus,
        Guid currentUserId,
        bool isAdmin
    );

    bool CancelOrder(Guid id, Guid currentUserId, bool isAdmin);
}
