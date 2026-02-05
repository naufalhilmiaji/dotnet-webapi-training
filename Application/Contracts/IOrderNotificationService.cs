namespace NhjDotnetApi.Application.Contracts;

public interface IOrderNotificationService
{
    Task NotifyStatusChangedAsync(
        Guid orderId,
        Guid userId,
        string status
    );
}
