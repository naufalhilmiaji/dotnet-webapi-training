using Microsoft.AspNetCore.SignalR;
using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Presentation.SignalR;

namespace NhjDotnetApi.Application.Services;

public class OrderNotificationService : IOrderNotificationService
{
    private readonly IHubContext<OrderHub, IOrderClient> _hub;

    public OrderNotificationService(
        IHubContext<OrderHub, IOrderClient> hub)
    {
        _hub = hub;
    }

    public async Task NotifyStatusChangedAsync(
        Guid orderId,
        Guid userId,
        string status)
    {
        // Notify order owner
        await _hub.Clients
            .Group($"user-{userId}")
            .OrderStatusUpdated(orderId, status);

        // Notify admins
        await _hub.Clients
            .Group("admins")
            .OrderStatusUpdated(orderId, status);
    }
}
