using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace NhjDotnetApi.Presentation.SignalR;

[Authorize] // JWT protected
public class OrderHub : Hub<IOrderClient>
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = Context.User?.FindFirstValue(ClaimTypes.Role);

        if (!string.IsNullOrEmpty(userId))
        {
            // Group per user (important for ownership)
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"user-{userId}"
            );
        }

        if (role == "ADMIN")
        {
            // Admin group (can receive all notifications)
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                "admins"
            );
        }
        Console.WriteLine("SignalR client connected");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                $"user-{userId}"
            );
        }

        await base.OnDisconnectedAsync(exception);
    }
}
